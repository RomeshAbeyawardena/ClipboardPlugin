using ClipboardPlugin.Abstractions.Expressions;
using ClipboardPlugin.Properties;

namespace ClipboardPlugin.Commands;

internal class VersionCommand(IIoStream ioStream, IExpressionEngine expressionEngine) : HelpContextCommandBase<ClipboardArguments>(DISPLAY_NAME)
{
    public const string DISPLAY_NAME = "version";

    public override async Task RenderContextHelpAsync(ClipboardArguments arguments, CancellationToken cancellationToken)
    {
        await ioStream.Out.WriteLineAsync(await ReplacePlaceholdersAsync(Resources.VersionHelp, expressionEngine));
    }

    public override bool CanExecute(ClipboardArguments arguments)
    {
        return arguments.Version;
    }

    public override async Task OnExecuteAsync(ClipboardArguments arguments, CancellationToken cancellationToken)
    {
        await ioStream.Out.WriteLineAsync(await ReplacePlaceholdersAsync(Resources.VersionInfo, expressionEngine));
    }
}
