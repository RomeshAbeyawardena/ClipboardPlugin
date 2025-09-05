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
        var bufferWidth = 32;
        var bufferHeight = 32;

        try
        {
            bufferWidth = Console.BufferWidth;
            bufferHeight = Console.BufferWidth;
        }
        catch(IOException)
        {

        }

        return new IoStream(Console.In, Console.Out, Console.Error, bufferWidth, bufferHeight);
    }
}
