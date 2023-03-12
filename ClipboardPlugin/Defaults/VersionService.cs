using ClipboardPlugin.Contracts;
using RST.Attributes;
using System.Reflection;

namespace ClipboardPlugin.Defaults;

[Register]
public class VersionService : IVersionService
{
    public Version? GetVersion()
    {
        return Assembly.GetExecutingAssembly().GetName().Version;
    }

    public string ReplaceVersion(string value, string? searchValue)
    {
        if (string.IsNullOrWhiteSpace(searchValue))
        {
            searchValue = "{version}";
        }

        return value.Replace(searchValue, GetVersion()!.ToString());
    }
}
