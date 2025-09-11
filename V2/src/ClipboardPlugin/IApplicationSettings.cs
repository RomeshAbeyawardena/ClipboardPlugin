namespace ClipboardPlugin;

public interface IApplicationSettings
{
    char StartPlaceholder { get; }
    char EndPlaceholder { get; }
    IEnumerable<char> KeyValueSeparators { get; }
}
