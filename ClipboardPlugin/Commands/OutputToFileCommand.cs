using ClipboardPlugin.Contracts;
using ClipboardPlugin.Extensions;
using ClipboardPlugin.Properties;
using Microsoft.Extensions.FileProviders;
using RST.Attributes;

namespace ClipboardPlugin.Commands;

[Register]
public class OutputToFileCommand : CommandBase
{
    private readonly IConsoleService consoleService;
    private readonly IFileWriter fileWriter;
    protected override Task<bool> OnCanExecute(CommandLineArguments arguments, string? commandName = null)
    {
        return this.CalculateCanExecute(arguments,
            !string.IsNullOrWhiteSpace(arguments.Output),
            string.IsNullOrWhiteSpace(arguments.Input));
    }

    public OutputToFileCommand(IConsoleService consoleService, IServiceProvider serviceProvider,
        IFileWriter fileWriter)
        : base(serviceProvider, "output", Resources.HelpText_Command_CopyToFile, CommandOrder.OUTPUT_COMMAND)
    {
        this.consoleService = consoleService;
        this.fileWriter = fileWriter;
    }

    public override async Task Execute(CommandLineArguments arguments, string? commandName = null)
    {
        var textToCopy = arguments.Text;
        try
        {
            if (!string.IsNullOrWhiteSpace(textToCopy) 
                && !string.IsNullOrWhiteSpace(arguments.Output))
            {
                await fileWriter.WriteAsync(arguments.Output, textToCopy);
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
