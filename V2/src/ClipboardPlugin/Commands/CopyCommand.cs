using ClipboardPlugin.Actions;
using ClipboardPlugin.Actions.Copying;
using ClipboardPlugin.Actions.Text;
using ClipboardPlugin.Properties;
using System.Text.RegularExpressions;

namespace ClipboardPlugin.Commands;

internal class CopyCommand(IIoStream ioStream, 
    IActionInvoker<CopyAction, ClipboardArguments> copyActionInvoker,
    IActionInvoker<TextAction, ClipboardArguments> textActionInvoker) : HelpContextCommandBase<ClipboardArguments>(DISPLAY_NAME, 1)
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

        var textActions = new List<TextAction>();
        if (arguments.IsReplacement)
        {
            textActions.Add(TextAction.Replace);
        }

        if (arguments.ExtractFileName)
        {
            textActions.Add(TextAction.ExtractFilename);
        }

        foreach (var textAction in textActions)
        {
            await textActionInvoker.ExecuteAsync(textAction, arguments, cancellationToken);
        }

        await ioStream.Out.WriteLineAsync($"Copying {arguments.Input} to {action}");
        await copyActionInvoker.ExecuteAsync(action, arguments, cancellationToken);
        
    }
}