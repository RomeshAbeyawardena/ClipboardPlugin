namespace ClipboardPlugin;

public record ClipboardArguments : IHelpContextArgument
{
    public ClipboardArguments(IDictionary<string, object> arguments)
    {
        arguments.AsModel(this, out _);
    }

    [Argument("t")]
    public string? Target { get; set; }

    public string? TargetKey
    {
        get
        {
            if (string.IsNullOrWhiteSpace(Target))
            {
                return null;
            }

            var idx =  Target.IndexOf('|');

            if(idx == -1)
            {
                return null;
            }

            return Target?[..idx];
        }
    }

    public string? TargetParameter
    {
        get
        {
            if (string.IsNullOrWhiteSpace(Target))
            {
                return null;
            }

            var idx = Target.IndexOf('|');

            if (idx == -1)
            {
                return null;
            }

            return Target?.Substring(idx+1, Target.Length - idx - 1);
        }
    }

    [Argument("f")]
    public string? Find { get; set; }

    [Argument("h")]
    public string? Replace { get; set; }

    [Argument("i")]
    public string? Input { get; set; }

    [Argument("?")]
    public bool Help { get; set; }

    [Argument("v")]
    public bool Version { get; set; }

    [Argument("r")]
    public bool Regex { get; set; }
}
