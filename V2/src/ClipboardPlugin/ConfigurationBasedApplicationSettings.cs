using Microsoft.Extensions.Configuration;

namespace ClipboardPlugin;

internal class ConfigurationBasedApplicationSettings : IApplicationSettings
{
    public ConfigurationBasedApplicationSettings(IConfiguration configuration)
    {
        configuration.Bind(this);
    }

    public required IEnumerable<char> KeyValueSeparators { get; set; }
}
