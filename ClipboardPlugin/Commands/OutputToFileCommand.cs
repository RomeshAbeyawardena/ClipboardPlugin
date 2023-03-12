using ClipboardPlugin.Contracts;
using ClipboardPlugin.Extensions;
using ClipboardPlugin.Properties;
using RST.Attributes;

namespace ClipboardPlugin.Commands;

[Register]
public class OutputToFileCommand : BaseCommand
{
    private readonly IConsoleService consoleService;

    protected override async Task<bool> OnCanExecute(CommandLineArguments arguments, string? commandName = null)
    {
        await Task.CompletedTask;
        return !arguments.Help.HasValue
            && !arguments.Version.HasValue
            && !string.IsNullOrWhiteSpace(arguments.Output)
            && !string.IsNullOrWhiteSpace(arguments.Text);
    }

    public OutputToFileCommand(IConsoleService consoleService, IServiceProvider serviceProvider)
        : base(serviceProvider, "output", Resources.HelpText_Command_CopyToFile)
    {
        this.consoleService = consoleService;
    }

    public override async Task Execute(CommandLineArguments arguments, string? commandName = null)
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
