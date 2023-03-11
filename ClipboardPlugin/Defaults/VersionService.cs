using ClipboardPlugin.Contracts;
using System.Reflection;

namespace ClipboardPlugin.Defaults;

internal class VersionService : IVersionService
{
    public Version? GetVersion()
    {
        return Assembly.GetExecutingAssembly().GetName().Version;
    }
}
