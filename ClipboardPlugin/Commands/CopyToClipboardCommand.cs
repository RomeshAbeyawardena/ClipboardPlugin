using ClipboardPlugin.Contracts;
using ClipboardPlugin.Extensions;

namespace ClipboardPlugin.Commands;

internal class CopyToClipboardCommand : ICommand
{
    private readonly IConsoleService consoleService;

    public CopyToClipboardCommand(IConsoleService consoleService)
    {
        Name = "text";
        this.consoleService = consoleService;
    }

    public string Name { get; }
    public string? HelpText { get; }

    public async Task<bool> CanExecute(CommandLineArguments arguments)
    {
        await Task.CompletedTask;
        return !arguments.Help.HasValue 
            && !arguments.Version.HasValue
            && string.IsNullOrWhiteSpace(arguments.Output)
            && !string.IsNullOrWhiteSpace(arguments.Text);
    }

    public async Task Execute(CommandLineArguments arguments)
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
                consoleService.WriteError(Properties.Resources.ErrorMessage_ContentIsNullOrEmptyOrAlreadyCopied);
        }
        else
            consoleService.WriteError(Properties.Resources.ErrorMessage_ContentIsNullOrEmpty);
    }
}
