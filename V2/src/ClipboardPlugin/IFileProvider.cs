namespace ClipboardPlugin;

internal interface IFileProvider
{
    Task<string> GetTextAsync(string fileName, CancellationToken cancellationToken);
    Task SetTextAsync(string filename, string content, CancellationToken cancellationToken);
}
