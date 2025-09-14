using ClipboardPlugin.Abstractions.Expressions;
using Microsoft.Extensions.Logging;
using NCalc;
using System.Collections.Concurrent;
using System.Globalization;

namespace ClipboardPlugin.ExpressionEngine;

public class ConfigurationExpressionEngine(TimeProvider timeProvider, ILogger<IExpressionEngine> logger,
    IPlaceholderScanner placeholderScanner, IApplicationSettings applicationSettings) : IExpressionEngine
{
    private ValueTask<DateTimeOffset> NowAsync(AsyncExpressionParameterData asyncExpressionParameter)
    {
        logger.LogInformation("{Id}", asyncExpressionParameter.Id);
        return ValueTask.FromResult(timeProvider.GetUtcNow());
    }

    public async Task<string> ResolveAsync(string value, CultureInfo culture)
    {
        var expressions = new ConcurrentQueue<(Range, string)>(placeholderScanner
            .GetPlaceholderExpressions(value, applicationSettings.StartPlaceholder, applicationSettings.EndPlaceholder));

        while(expressions.TryDequeue(out var item))
        {
            var (range, expression) = item;
        
            if (string.IsNullOrWhiteSpace(expression))
            {
                continue;
            }

            var result = await Expression(expression, culture).EvaluateAsync();

            if (result is null)
            {
                continue;
            }

            value = value.Remove(range.Start.Value, expression.Length + 2);
            value = value.Insert(range.Start.Value, result.ToString() ?? string.Empty);
        }

        return value;
    }

    public AsyncExpression Expression(string expression, CultureInfo culture)
    {
        var expr =  new AsyncExpression(expression, ExpressionOptions.None, culture);
        expr.DynamicParameters.Add("now", async (a) => await NowAsync(a));

        expr.Functions.Add("parseDate", async (x) => {
            var value = x.FirstOrDefault() ?? throw new ArgumentNullException(nameof(expression));
            var format = x.ElementAtOrDefault(1) ?? throw new ArgumentNullException(nameof(expression));

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
        return expr;
    }
}
