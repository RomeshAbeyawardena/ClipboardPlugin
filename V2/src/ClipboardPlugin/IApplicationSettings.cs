namespace ClipboardPlugin;

public interface IApplicationSettings
{
    IEnumerable<char> KeyValueSeparators { get; }
}
