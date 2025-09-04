
using ClipboardPlugin.Properties;

namespace ClipboardPlugin.Commands;

internal class HelpCommand(IIoStream ioStream) : CommandBase<ClipboardArguments>
{
    private Task RenderHelp(ClipboardArguments arguments, CancellationToken cancellationToken)
    {
        return ioStream.Out.WriteLineAsync(Resources.GeneralHelp);
    }

    public override bool CanExecute(ClipboardArguments arguments)
    {
        return arguments.Help && string.IsNullOrWhiteSpace(arguments.Source) && string.IsNullOrWhiteSpace(arguments.Target);
    }

    public override async Task ExecuteAsync(ClipboardArguments arguments, CancellationToken cancellationToken)
    {
        await RenderHelp(arguments, cancellationToken);
    }
}
