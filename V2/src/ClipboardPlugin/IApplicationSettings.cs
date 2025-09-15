namespace ClipboardPlugin;

public interface IApplicationSettings
{
    int? RecursionLevel { get; }
    char StartPlaceholder { get; }
    char EndPlaceholder { get; }
    IEnumerable<char> KeyValueSeparators { get; }
}
