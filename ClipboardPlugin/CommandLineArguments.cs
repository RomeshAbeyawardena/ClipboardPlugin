using Microsoft.Extensions.Configuration;

namespace ClipboardPlugin;

public record CommandLineArguments
{
    public CommandLineArguments(IConfiguration configuration)
    {
        configuration.Bind(this);
    }
    public string? Text { get; set; }
}
