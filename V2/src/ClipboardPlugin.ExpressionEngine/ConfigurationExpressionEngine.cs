using ClipboardPlugin.Abstractions.Expressions;
using NCalc;
using System.Collections.Concurrent;
using System.Globalization;

namespace ClipboardPlugin.ExpressionEngine;

public class ConfigurationExpressionEngine : IExpressionEngine
{
    private readonly AsyncExpressionContext _expressionContext;
    private readonly TimeProvider _timeProvider;
    private readonly IPlaceholderScanner _placeholderScanner;
    private readonly IApplicationSettings _applicationSettings;
    
    public ConfigurationExpressionEngine(TimeProvider timeProvider, IPlaceholderScanner placeholderScanner, IApplicationSettings applicationSettings, CultureInfo culture)
    {
        _timeProvider = timeProvider;
        _placeholderScanner = placeholderScanner;
        _applicationSettings = applicationSettings;
        
        _expressionContext =  new AsyncExpressionContext(ExpressionOptions.None, culture);
        _expressionContext.DynamicParameters.Add("now", async (a) => await NowAsync(a));

        _expressionContext.Functions.Add("parseDate", async (x) => {
            var value = x.FirstOrDefault() ?? throw new ArgumentNullException(nameof(x));
            var format = x.ElementAtOrDefault(1) ?? throw new ArgumentNullException(nameof(x));

            var v = await value.EvaluateAsync();
            var f = await format.EvaluateAsync();
            var formatStr = f?.ToString();
            if (DateTimeOffset.TryParse(v?.ToString(), culture, out var m))
            {
                return !string.IsNullOrWhiteSpace(formatStr)
                ? m.ToString(formatStr, culture)
                : m.ToString(culture);
            }

            return "?INVALID_DATE!";
        });
    }

    private ValueTask<DateTimeOffset> NowAsync(AsyncExpressionParameterData asyncExpressionParameter)
    {
        //_logger.LogInformation("{Id}", asyncExpressionParameter.Id);
        return ValueTask.FromResult(_timeProvider.GetUtcNow());
    }

    int currentRecursion = 0;
    public async Task<string> ResolveAsync (string value)
    {
        var expressions = new ConcurrentQueue<(Range, string)>(_placeholderScanner
            .GetPlaceholderExpressions(value, _applicationSettings.StartPlaceholder, _applicationSettings.EndPlaceholder));
        
        while(expressions.TryDequeue(out var item))
        {
            var (range, expression) = item;
        
            if (string.IsNullOrWhiteSpace(expression))
            {
                continue;
            }

            var result = await new AsyncExpression(expression, _expressionContext).EvaluateAsync();

            if (result is null)
            {
                continue;
            }

            value = value.Remove(range.Start.Value, expression.Length + 2);
            value = value.Insert(range.Start.Value, result.ToString() ?? string.Empty);
        }

        if (currentRecursion++ < 20)
        {
            value = await ResolveAsync(value);
        }
        else
        {
            currentRecursion = 0;
        }

        return value;
    }

    public void Extend(Action<ExpressionContextBase> config)
    {
        config(_expressionContext);
    }
}
