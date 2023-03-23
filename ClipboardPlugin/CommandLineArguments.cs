using Microsoft.Extensions.Configuration;

namespace ClipboardPlugin;

public record CommandLineArguments
{
    public CommandLineArguments(IConfiguration configuration)
    {
        configuration.Bind(this);
    }

    public string? HelpContext { get; set; }
    public bool? Help { get; set; }
    public string? Input { get; set; }
    public int? Index { get; set; }
    public string? Output { get; set; }
    public string? SplitString { get; set; }
    public string? Text { get; set; }
    public bool? Version { get; set; }
    public string? SearchString { get; set; }
    public string? ReplacementString { get; set; }
    public string? PrependValue { get; set; }
    public string? AppendValue { get; set; }
    public bool Async { get; set; }
    public bool Process { get; set; } = true;
}
