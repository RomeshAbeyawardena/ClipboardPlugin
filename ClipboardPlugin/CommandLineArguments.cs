using Microsoft.Extensions.Configuration;

namespace ClipboardPlugin;

public record CommandLineArguments
{
    public CommandLineArguments(IConfiguration configuration)
    {
        configuration.Bind(this);
    }


    public bool? Help { get; set; }
    public int? Index { get; set; }
    public string? Output { get; set; }
    public string? SplitString { get; set; }
    public string? Text { get; set; }
    public bool? Version { get; set; }
}
