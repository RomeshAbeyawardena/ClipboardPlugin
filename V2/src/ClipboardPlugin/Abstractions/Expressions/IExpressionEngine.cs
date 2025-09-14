using NCalc;

namespace ClipboardPlugin.Abstractions.Expressions;

public interface IExpressionEngine
{
    void Extend(Action<ExpressionContextBase> expressionContext);
    Task<string> ResolveAsync(string value);
}
