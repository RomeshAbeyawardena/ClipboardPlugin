using NCalc;
namespace ClipboardPlugin.ExpressionEngine;

public class ConfigurationExpressionEngine(TimeProvider timeProvider)
{
    private ValueTask<DateTimeOffset> Now(AsyncExpressionParameterData asyncExpressionParameter)
    {
        return ValueTask.FromResult(timeProvider.GetUtcNow());
    }

    public AsyncExpression Expression(string expression)
    {
        var expr =  new AsyncExpression(expression, ExpressionOptions.None, System.Globalization.CultureInfo.CurrentCulture);
        expr.DynamicParameters.Add("now", async (a) => await Now(a));
        return expr;
    }
}
