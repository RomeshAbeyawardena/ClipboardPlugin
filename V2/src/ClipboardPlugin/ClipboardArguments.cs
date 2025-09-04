namespace ClipboardPlugin;

public record ClipboardArguments
{
    public ClipboardArguments(IDictionary<string, object> arguments)
    {
        arguments.AsModel(this, out _);
    }

    [Argument("h", "?")]
    public bool Help { get; set; }

    [Argument("v")]
    public bool Version { get; set; }
}
