namespace ClipboardPlugin.Abstractions.Expressions;

public interface IPlaceholderScanner
{
    IEnumerable<(Range, string)> GetPlaceholderExpressions(string value, char startChar, char endChar);
    IEnumerable<Range> ScanRanges(string value, char startChar, char endChar);
}
