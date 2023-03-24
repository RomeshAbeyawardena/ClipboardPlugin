namespace ClipboardPlugin.Contracts;

public interface IFileWriter
{
    void Write(string fileName, string content);
    Task WriteAsync(string fileName, string content, CancellationToken? cancellationToken = default);
}
