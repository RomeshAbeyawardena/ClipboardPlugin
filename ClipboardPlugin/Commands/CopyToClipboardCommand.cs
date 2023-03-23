using ClipboardPlugin.Contracts;
using ClipboardPlugin.Extensions;
using ClipboardPlugin.Properties;
using RST.Attributes;

namespace ClipboardPlugin.Commands;

[Register]
public class CopyToClipboardCommand : BaseCommand
{
    private readonly IConsoleService consoleService;

    protected override Task<bool> OnCanExecute(CommandLineArguments arguments, string? command = null)
    {
        return this.CalculateCanExecute(arguments, string.IsNullOrWhiteSpace(arguments.Output));
    }

    public CopyToClipboardCommand(IConsoleService consoleService, IServiceProvider serviceProvider)
        : base(serviceProvider, "copy", Resources.HelpText_Command_CopyToClipboard, int.MaxValue)
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
            var textToCopy = arguments.HandleTextProcessing();

            if (!string.IsNullOrWhiteSpace(textToCopy) && !textToCopy.Equals(currentClipboard))
                await TextCopy.ClipboardService.SetTextAsync(textToCopy);
            else
                consoleService.WriteError(Resources.ErrorMessage_ContentIsNullOrEmptyOrAlreadyCopied);
        }
        else
            consoleService.WriteError(Properties.Resources.ErrorMessage_ContentIsNullOrEmpty);
    }
}
