using ClipboardPlugin.Contracts;
using ClipboardPlugin.Extensions;
using ClipboardPlugin.Properties;
using RST.Attributes;

namespace ClipboardPlugin.Commands;

[Register]
public class OutputToFileCommand : CommandBase
{
    private readonly IConsoleService consoleService;

    protected override Task<bool> OnCanExecute(CommandLineArguments arguments, string? commandName = null)
    {
        return this.CalculateCanExecute(arguments,
            !string.IsNullOrWhiteSpace(arguments.Output),
            !string.IsNullOrWhiteSpace(arguments.Input));
    }

    public OutputToFileCommand(IConsoleService consoleService, IServiceProvider serviceProvider)
        : base(serviceProvider, "output", Resources.HelpText_Command_CopyToFile, int.MaxValue)
    {
        this.consoleService = consoleService;
    }

    public override async Task Execute(CommandLineArguments arguments, string? commandName = null)
    {
        var textToCopy = arguments.Text;
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
