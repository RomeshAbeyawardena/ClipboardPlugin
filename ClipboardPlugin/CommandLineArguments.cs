using Microsoft.Extensions.Configuration;

namespace ClipboardPlugin;

public record CommandLineArguments
{
    public CommandLineArguments(IConfiguration configuration)
    {
        configuration.Bind(this);
    }

    public bool? Help { get; set; }
    public string? Text { get; set; }
}
