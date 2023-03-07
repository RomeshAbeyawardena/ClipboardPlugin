using Microsoft.Extensions.Configuration.CommandLine;
using Microsoft.Extensions.Configuration;

namespace ClipboardPlugin;

public partial class Program
{
    private static readonly ConfigurationBuilder cb = new ConfigurationBuilder();
    private static readonly Dictionary<string, string> mappings = new Dictionary<string, string>() { { "-t", "Text" }, { "--text", "Text" } };
    private static IConfiguration configuration; 
    private static CommandLineArguments commandLineArguments;


    private static void WriteColouredText(string message, ConsoleColor consoleColor, params object[] args)
    {
        var original = Console.ForegroundColor;
        Console.ForegroundColor = consoleColor;
        Console.WriteLine(message, args);
        Console.ForegroundColor = original;
    }

    private static void WriteError(string message, params object[] args)
    {
        WriteColouredText($"ERROR: {message}", ConsoleColor.Red, args);
    }

    private static void WriteDebug(string message, params object[] args)
    {
#if DEBUG
        WriteColouredText($"DEBUG: {message}", ConsoleColor.DarkCyan, args);
#endif
    }

    public async static Task<int> Main(string[] args)
    {
        configuration = cb.Add(new CommandLineConfigurationSource() { Args = args, SwitchMappings = mappings }).Build();
        commandLineArguments = new CommandLineArguments(configuration);
        await CopyText();
        return 0;
    }
}
