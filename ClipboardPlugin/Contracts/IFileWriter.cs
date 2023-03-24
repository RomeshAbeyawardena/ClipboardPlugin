namespace ClipboardPlugin.Contracts;

/// <summary>
/// Represents a file writer
/// </summary>
public interface IFileWriter
{
    /// <summary>
    /// Writes <paramref name="content"/> to a file specified in <paramref name="fileName"/>
    /// </summary>
    /// <param name="fileName">Fully qualified path to store content</param>
    /// <param name="content">The content to store</param>
    void Write(string fileName, string content);
    /// <summary>
    /// Writes <paramref name="content"/> to a file specified in <paramref name="fileName"/>
    /// </summary>
    /// <param name="fileName">Fully qualified path to store content</param>
    /// <param name="content">The content to store</param>
    /// <param name="cancellationToken"></param>
    /// <returns><inheritdoc cref="File.WriteAllTextAsync(string, string?, CancellationToken)"/></returns>
    Task WriteAsync(string fileName, string content, CancellationToken? cancellationToken = default);
}
