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
            
            return true;
        });
        return expr;
    }
}
