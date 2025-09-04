
using ClipboardPlugin.Actions;
using ClipboardPlugin.Properties;
using TextCopy;

namespace ClipboardPlugin.Commands;

internal class CopyCommand(IIoStream ioStream, IActionInvoker<CopyAction> copyActionInvoker) : HelpContextCommandBase<ClipboardArguments>(1)
{
    public override bool CanExecute(ClipboardArguments arguments)
    {
        return !string.IsNullOrWhiteSpace(arguments.Source);
    }

    public override Task RenderContextHelpAsync(ClipboardArguments arguments, CancellationToken cancellationToken)
    {
        return ioStream.Out.WriteLineAsync(Resources.CopyHelp);
    }

    public override async Task OnExecuteAsync(ClipboardArguments arguments, CancellationToken cancellationToken)
    {
        await ioStream.Out.WriteLineAsync($"{arguments.Source} {arguments.Target}");

        if (!Enum.TryParse<CopyAction>(arguments.Target, out var action))
        {
            action = CopyAction.Clipboard;
        }

        await copyActionInvoker.ExecuteAsync(action, cancellationToken);
        
    }
}