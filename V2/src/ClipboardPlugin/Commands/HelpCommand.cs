using ClipboardPlugin.Properties;
using Microsoft.Extensions.DependencyInjection;

namespace ClipboardPlugin.Commands;

internal class HelpCommand(IIoStream ioStream, IServiceProvider serviceProvider) : CommandBase<ClipboardArguments>("help")
{
    private async Task RenderHelp(CancellationToken cancellationToken)
    {
        var commands = serviceProvider.GetServices<ICommand<ClipboardArguments>>();
        await ioStream.Out.WriteLineAsync(Resources.GeneralHelp);
        foreach (var command in commands.OrderBy(x => x.Priority))
        {
            if(command is HelpContextCommandBase<ClipboardArguments> helpContext)
            {
                await ioStream.Out.WriteAsync(new string('-', Console.BufferWidth));
                await helpContext.RenderContextHelpAsync(null!, cancellationToken);
            }
        }
    }

    public override bool CanExecute(ClipboardArguments arguments)
    {
        return arguments.Help;
    }

    public override async Task ExecuteAsync(ClipboardArguments arguments, CancellationToken cancellationToken)
    {
        await RenderHelp(cancellationToken);
    }
}
