using ClipboardPlugin.Properties;

namespace ClipboardPlugin.Commands;

internal class VersionCommand(IIoStream ioStream) : HelpContextCommandBase<ClipboardArguments>("version")
{
    public override Task RenderContextHelpAsync(ClipboardArguments arguments, CancellationToken cancellationToken)
    {
        return ioStream.Out.WriteLineAsync(ReplacePlaceholders(Resources.VersionHelp));
    }

    public override bool CanExecute(ClipboardArguments arguments)
    {
        return arguments.Version;
    }

    public override async Task OnExecuteAsync(ClipboardArguments arguments, CancellationToken cancellationToken)
    {
        await ioStream.Out.WriteLineAsync(ReplacePlaceholders(Resources.VersionInfo));
    }
}
