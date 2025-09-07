using NCalc;
namespace ClipboardPlugin.ExpressionEngine;

public record ConfigurationExpressionContext : ExpressionContextBase
{

}

public class ConfigurationExpressionEngine : ExpressionBase<ConfigurationExpressionContext>
{
    public NCalc.AsyncExpression Expression(string expression)
    {
        return new AsyncExpression(expression, ExpressionOptions.None, System.Globalization.CultureInfo.CurrentCulture);
    }
}
