
using ClipboardPlugin.Properties;

namespace ClipboardPlugin.Commands;

internal class CopyCommand(IIoStream ioStream) : HelpContextCommandBase<ClipboardArguments>(1)
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
    }
}
