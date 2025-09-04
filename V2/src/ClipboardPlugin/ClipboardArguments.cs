namespace ClipboardPlugin;

public record ClipboardArguments : IHelpContextArgument
{
    public ClipboardArguments(IDictionary<string, object> arguments)
    {
        arguments.AsModel(this, out _);
    }

    [Argument("t")]
    public string? Target { get; set; }

    [Argument("i")]
    public string? Input { get; set; }

    [Argument("h", "?")]
    public bool Help { get; set; }

    [Argument("v")]
    public bool Version { get; set; }
}
