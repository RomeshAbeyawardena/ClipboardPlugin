using ClipboardPlugin.Contracts;
using ClipboardPlugin.Extensions;

namespace ClipboardPlugin.Commands;

internal class OutputToFileCommand : ICommand
{
    private readonly IConsoleService consoleService;

    public OutputToFileCommand(IConsoleService consoleService)
    {
        Name = "output";
        this.consoleService = consoleService;
    }

    public string Name { get; }
    public string? HelpText { get; }

    public async Task<bool> CanExecute(CommandLineArguments arguments)
    {
        await Task.CompletedTask;
        return !arguments.Help.HasValue
            && !arguments.Version.HasValue
            && !string.IsNullOrWhiteSpace(arguments.Output)
            && !string.IsNullOrWhiteSpace(arguments.Text);
    }

    public async Task Execute(CommandLineArguments arguments)
    {
        var textToCopy = arguments.HandleTextProcessing();
        try
        {
            if (!string.IsNullOrWhiteSpace(textToCopy) 
                && !string.IsNullOrWhiteSpace(arguments.Output))
            {
                await File.WriteAllTextAsync(arguments.Output, textToCopy);
                Console.WriteLine($"Saved to file: \"{arguments.Output}\"");
            }
            else
            {
                Console.WriteLine("Nothing to save!");
            }
        }
        catch(IOException exception)
        {
            consoleService.WriteError(exception.Message);
        }
    }
}
