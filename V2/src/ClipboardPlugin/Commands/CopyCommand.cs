using ClipboardPlugin.Actions;
using ClipboardPlugin.Actions.Copying;
using ClipboardPlugin.Properties;
using System.Text.RegularExpressions;

namespace ClipboardPlugin.Commands;

internal class CopyCommand(IIoStream ioStream, IActionInvoker<CopyAction, ClipboardArguments> copyActionInvoker) : HelpContextCommandBase<ClipboardArguments>(DISPLAY_NAME, 1)
{
    public const string DISPLAY_NAME = "copy";
    public override bool CanExecute(ClipboardArguments arguments)
    {
        return !string.IsNullOrWhiteSpace(arguments.Input);
    }

    public override Task RenderContextHelpAsync(ClipboardArguments arguments, CancellationToken cancellationToken)
    {
        return ioStream.Out.WriteLineAsync(ReplacePlaceholders(Resources.CopyHelp));
    }

    public override async Task OnExecuteAsync(ClipboardArguments arguments, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(arguments.Input);

        if (!Enum.TryParse<CopyAction>(arguments.TargetKey, true, out var action))
        {
            action = CopyAction.Clipboard;
        }

        if (!string.IsNullOrWhiteSpace(arguments.Find))
        {
            arguments.Input = arguments.Regex 
                ? Regex.Replace(arguments.Input, arguments.Find, arguments.Replace ?? string.Empty)
                : arguments.Input.Replace(arguments.Find, arguments.Replace);
        }

        await ioStream.Out.WriteLineAsync($"Copying {arguments.Input} to {action}");

        await copyActionInvoker.ExecuteAsync(action, arguments, cancellationToken);
        
    }
}