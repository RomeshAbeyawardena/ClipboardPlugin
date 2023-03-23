using ClipboardPlugin.Contracts;
using ClipboardPlugin.Extensions;
using ClipboardPlugin.Properties;
using RST.Attributes;

namespace ClipboardPlugin.Commands;

[Register]
public class OutputToClipboardCommand : CommandBase
{
    private readonly IConsoleService consoleService;

    protected override Task<bool> OnCanExecute(CommandLineArguments arguments, string? command = null)
    {
        return this.CalculateCanExecute(arguments, 
            string.IsNullOrWhiteSpace(arguments.Output),
            !string.IsNullOrWhiteSpace(arguments.Input));
    }

    public OutputToClipboardCommand(IConsoleService consoleService, IServiceProvider serviceProvider)
        : base(serviceProvider, "copy", Resources.HelpText_Command_CopyToClipboard, CommandOrder.OUTPUT_COMMAND)
    {
        this.consoleService = consoleService;
    }

    public override async Task Execute(CommandLineArguments arguments, string? commandName = null)
    {
        await Task.CompletedTask;
        consoleService.WriteDebug("{0}", arguments);
        var currentClipboard = await TextCopy.ClipboardService.GetTextAsync();
        if (!string.IsNullOrWhiteSpace(arguments!.Text))
        {
            var textToCopy = arguments.Text;

            if (!string.IsNullOrWhiteSpace(textToCopy) && !textToCopy.Equals(currentClipboard))
                await TextCopy.ClipboardService.SetTextAsync(textToCopy);
            else
                consoleService.WriteError(Resources.ErrorMessage_ContentIsNullOrEmptyOrAlreadyCopied);
        }
        else
            consoleService.WriteError(Properties.Resources.ErrorMessage_ContentIsNullOrEmpty);
    }
}
