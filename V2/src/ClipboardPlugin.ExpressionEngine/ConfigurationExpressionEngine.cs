using NCalc;
using System.Globalization;
namespace ClipboardPlugin.ExpressionEngine;

public class ConfigurationExpressionEngine(TimeProvider timeProvider)
{
    private ValueTask<DateTimeOffset> NowAsync(AsyncExpressionParameterData asyncExpressionParameter)
    {
        return ValueTask.FromResult(timeProvider.GetUtcNow());
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
