
namespace ClipboardPlugin.Commands;

internal class CopyCommand(IIoStream ioStream) : CommandBase<ClipboardArguments>
{
    public override bool CanExecute(ClipboardArguments arguments)
    {
        return !string.IsNullOrWhiteSpace(arguments.Source);
    }

    public override async Task ExecuteAsync(ClipboardArguments arguments, CancellationToken cancellationToken)
    {
        await ioStream.Out.WriteLineAsync($"{arguments.Source} {arguments.Target}");
    }
}
