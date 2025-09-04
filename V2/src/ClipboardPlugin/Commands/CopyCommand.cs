using ClipboardPlugin.Actions;
using ClipboardPlugin.Properties;

namespace ClipboardPlugin.Commands;

internal class CopyCommand(IIoStream ioStream, IActionInvoker<CopyAction, ClipboardArguments> copyActionInvoker) : HelpContextCommandBase<ClipboardArguments>(1)
{
    public override bool CanExecute(ClipboardArguments arguments)
    {
        return !string.IsNullOrWhiteSpace(arguments.Input);
    }

    public override Task RenderContextHelpAsync(ClipboardArguments arguments, CancellationToken cancellationToken)
    {
        return ioStream.Out.WriteLineAsync(Resources.CopyHelp);
    }

    public override async Task OnExecuteAsync(ClipboardArguments arguments, CancellationToken cancellationToken)
    {
        await ioStream.Out.WriteLineAsync($"{arguments.Input} {arguments.Target}");

        if (!Enum.TryParse<CopyAction>(arguments.TargetKey, true, out var action))
        {
            action = CopyAction.Clipboard;
        }

        await copyActionInvoker.ExecuteAsync(action, arguments, cancellationToken);
        
    }
}