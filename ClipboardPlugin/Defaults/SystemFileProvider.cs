using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Internal;
using Microsoft.Extensions.FileProviders.Physical;
using Microsoft.Extensions.Primitives;

namespace ClipboardPlugin.Defaults;

public class SystemFileProvider : IFileProvider
{
    public IDirectoryContents GetDirectoryContents(string subpath)
    {
        return new PhysicalDirectoryContents(subpath);
    }

    public IFileInfo GetFileInfo(string subpath)
    {
        return new PhysicalFileInfo(new FileInfo(subpath));
    }

    public IChangeToken Watch(string filter)
    {
        throw new NotImplementedException();
    }
}
