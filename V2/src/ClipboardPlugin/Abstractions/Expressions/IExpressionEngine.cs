using System.Globalization;

namespace ClipboardPlugin.Abstractions.Expressions;

public interface IExpressionEngine
{
    Task<string> Resolve(string value, CultureInfo culture);
}
