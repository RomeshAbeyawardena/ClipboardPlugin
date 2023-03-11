using ClipboardPlugin.Contracts;

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
            && !string.IsNullOrWhiteSpace(arguments.Text);
    }

    public async Task Execute(CommandLineArguments arguments)
    {
        await Task.CompletedTask;
        consoleService.WriteDebug("{0}", arguments);
        var currentClipboard = await TextCopy.ClipboardService.GetTextAsync();
        if (!string.IsNullOrWhiteSpace(arguments!.Text))
        {
            var textToCopy = arguments.Text;
            if (!string.IsNullOrEmpty(arguments.SplitString))
            {
                var splitString = arguments.Text.Split(arguments.SplitString);
                if (arguments.Index.HasValue)
                {
                    if (arguments.Index != -1)
                    {
                        textToCopy = splitString.ElementAtOrDefault(arguments.Index.Value);
                    }
                    else
                        textToCopy = splitString.LastOrDefault();
                }
                else
                    textToCopy = string.Join(",", splitString);
            }
            if (!string.IsNullOrWhiteSpace(textToCopy) && !textToCopy.Equals(currentClipboard))
                await TextCopy.ClipboardService.SetTextAsync(textToCopy);
            else
                consoleService.WriteError(Properties.Resources.ErrorMessage_ContentIsNullOrEmptyOrAlreadyCopied);
        }
        else
            consoleService.WriteError(Properties.Resources.ErrorMessage_ContentIsNullOrEmpty);
    }
}
