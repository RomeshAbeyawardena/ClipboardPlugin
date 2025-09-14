using System.Globalization;

namespace ClipboardPlugin.Abstractions.Expressions;

public interface IExpressionEngine
{
    Task<string> ResolveAsync(string value);
}
