namespace ClipboardPlugin;

public interface IIoStream
{
    public TextReader In { get; }
    public TextWriter Out { get; }
    public TextWriter Error { get; }
}

public record IoStream(TextReader In, TextWriter Out, TextWriter Error) : IIoStream
{
    public static IIoStream ConsoleStream()
    {
        return new IoStream(Console.In, Console.Out, Console.Error);
    }
}
