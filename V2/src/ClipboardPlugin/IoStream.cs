namespace ClipboardPlugin;

public interface IIoStream
{
    public TextReader In { get; }
    public TextWriter Out { get; }
    public TextWriter Error { get; }
    public int BufferWidth { get; }
    public int BufferHeight { get; }
}

public record IoStream(TextReader In, TextWriter Out, TextWriter Error, int BufferWidth, int BufferHeight) : IIoStream
{
    public static IIoStream ConsoleStream()
    {
        return new IoStream(Console.In, Console.Out, Console.Error, Console.BufferWidth, Console.BufferWidth);
    }
}
