// See https://aka.ms/new-console-template for more information
using ClipboardPlugin;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.CommandLine;
var cb = new ConfigurationBuilder();
var mappings = new Dictionary<string, string>() { { "-t", "Text" }, { "--text", "Text" } };
var configuration = cb.Add(new CommandLineConfigurationSource() { Args = args, SwitchMappings = mappings }).Build();
var commandLineArguments = new CommandLineArguments(configuration);

void WriteColouredText(string message, ConsoleColor consoleColor, params object[] args)
{
    var original = Console.ForegroundColor;
    Console.ForegroundColor = consoleColor;
    Console.WriteLine(message, args);
    Console.ForegroundColor = original;
}

void WriteError(string message, params object[] args)
{
    WriteColouredText($"ERROR: {message}", ConsoleColor.Red, args);
}

void WriteDebug(string message, params object[] args)
{
#if DEBUG
    WriteColouredText($"DEBUG: {message}", ConsoleColor.DarkCyan, args);
#endif
}

WriteDebug("{0}", commandLineArguments);

if (!string.IsNullOrWhiteSpace(commandLineArguments.Text))
{
    await TextCopy.ClipboardService.SetTextAsync(commandLineArguments.Text);
}
else
    WriteError("Aborted: No text to copy");
return 0;