namespace ClipboardPlugin;

public record ClipboardArguments
{
    public ClipboardArguments(IDictionary<string, object> arguments)
    {
        arguments.AsModel(this);
    }
}
