namespace ClipboardPlugin;

internal interface IFileProvider
{
    Task<string> GetTextAsync(string fileName, CancellationToken cancellationToken);
    Task SetTextAsync(string filename, string content, CancellationToken cancellationToken);
}

public class PhysicalFileProvider : IFileProvider
{
    public Task<string> GetTextAsync(string fileName, CancellationToken cancellationToken)
    {
        return File.ReadAllTextAsync(fileName, cancellationToken);
    }

    public Task SetTextAsync(string filename, string content, CancellationToken cancellationToken)
    {
        return File.WriteAllTextAsync(filename, content, cancellationToken);
    }
}
