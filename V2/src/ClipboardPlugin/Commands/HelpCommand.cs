
namespace ClipboardPlugin.Commands;

internal class HelpCommand(IIoStream ioStream) : CommandBase<ClipboardArguments>
{
    private async Task RenderHelp(ClipboardArguments arguments, CancellationToken cancellationToken)
    {

    }

    public override bool CanExecute(ClipboardArguments arguments)
    {
        
        return arguments.Help && string.IsNullOrWhiteSpace(arguments.Source) && string.IsNullOrWhiteSpace(arguments.Target);
    }

    public override async Task ExecuteAsync(ClipboardArguments arguments, CancellationToken cancellationToken)
    {
        await ioStream.Out.WriteLineAsync("HELP");
    }
}
