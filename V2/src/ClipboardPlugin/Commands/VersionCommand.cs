using ClipboardPlugin.Properties;

namespace ClipboardPlugin.Commands;

internal class VersionCommand(IIoStream ioStream) : CommandBase<ClipboardArguments>
{
    public override bool CanExecute(ClipboardArguments arguments)
    {
        return arguments.Version;
    }
    public override async Task ExecuteAsync(ClipboardArguments arguments, CancellationToken cancellationToken)
    {
        await ioStream.Out.WriteLineAsync(Resources.VersionInfo);
    }
}
