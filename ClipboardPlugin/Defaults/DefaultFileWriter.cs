using ClipboardPlugin.Contracts;
using RST.Attributes;

namespace ClipboardPlugin.Defaults;

[Register]
public class DefaultFileWriter : IFileWriter
{
    public void Write(string fileName, string content)
    {
        File.WriteAllText(fileName, content);
    }

    public Task WriteAsync(string fileName, string content, CancellationToken? cancellationToken = null)
    {
        return File.WriteAllTextAsync(fileName, content, cancellationToken ?? CancellationToken.None);
    }
}
