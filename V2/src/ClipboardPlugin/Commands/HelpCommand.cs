using ClipboardPlugin.Abstractions.Expressions;
using ClipboardPlugin.Properties;
using Microsoft.Extensions.DependencyInjection;

namespace ClipboardPlugin.Commands;

internal class HelpCommand(IIoStream ioStream, IServiceProvider serviceProvider, IExpressionEngine expressionEngine) : CommandBase<ClipboardArguments>(DISPLAY_NAME)
{
    public const string DISPLAY_NAME = "help";
    private async Task RenderHelp(CancellationToken cancellationToken)
    {
        var commands = serviceProvider.GetServices<ICommand<ClipboardArguments>>();
        await ioStream.Out.WriteLineAsync(await ReplacePlaceholders(Resources.GeneralHelp, expressionEngine));
        foreach (var command in commands.OrderBy(x => x.Priority))
        {
            if(command is HelpContextCommandBase<ClipboardArguments> helpContext)
            {
                var commandName = Humanizer.CasingExtensions.ApplyCase(command.Name, Humanizer.LetterCasing.Title);
                await ioStream.Out.WriteLineAsync(new string('-', ioStream.BufferWidth));
                await ioStream.Out.WriteAsync($"{commandName}: ");
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
