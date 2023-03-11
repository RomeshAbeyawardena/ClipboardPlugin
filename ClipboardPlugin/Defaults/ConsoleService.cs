using ClipboardPlugin.Contracts;

namespace ClipboardPlugin.Defaults;

internal class ConsoleService : IConsoleService
{
    private static void WriteColouredText(string message, ConsoleColor consoleColor, params object[] args)
    {
        var original = Console.ForegroundColor;
        Console.ForegroundColor = consoleColor;
        Console.WriteLine(message, args);
        Console.ForegroundColor = original;
    }

    public void WriteDebug(string message, params object[] args)
    {
        WriteColouredText($"DEBUG: {message}", ConsoleColor.DarkCyan, args);
    }

    public void WriteError(string message, params object[] args)
    {
        WriteColouredText($"ERROR: {message}", ConsoleColor.Red, args);
    }
}
