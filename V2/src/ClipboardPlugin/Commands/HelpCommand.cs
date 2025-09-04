
namespace ClipboardPlugin.Commands;

internal class HelpCommand(IIoStream ioStream) : CommandBase<ClipboardArguments>
{
    public override bool CanExecute(ClipboardArguments arguments)
    {
        return arguments.Help;
    }

    public override async Task ExecuteAsync(ClipboardArguments arguments, CancellationToken cancellationToken)
    {
        await ioStream.Out.WriteLineAsync("HELP");
    }
}
