using ClipboardPlugin.Abstractions.Expressions;
using ClipboardPlugin.Actions;
using ClipboardPlugin.Actions.Copying;
using ClipboardPlugin.Actions.Text;
using ClipboardPlugin.Extensions;
using ClipboardPlugin.Properties;
using ClipboardPlugin.Repositories;

namespace ClipboardPlugin.Commands;

internal class CopyCommand(IIoStream ioStream, IApplicationSettings applicationSettings, IExpressionEngine expressionEngine,
    IActionInvoker<CopyAction, ClipboardArguments> copyActionInvoker,
    IActionInvoker<TextAction, ClipboardArguments> textActionInvoker,
    IKeyValueRepository keyValueRepository) : HelpContextCommandBase<ClipboardArguments>(DISPLAY_NAME, 1)
{
    public const string DISPLAY_NAME = "copy";
    public override bool CanExecute(ClipboardArguments arguments)
    {
        return !string.IsNullOrWhiteSpace(arguments.Input);
    }

    public override async Task RenderContextHelpAsync(ClipboardArguments arguments, CancellationToken cancellationToken)
    {
        await ioStream.Out.WriteLineAsync(await ReplacePlaceholdersAsync(Resources.CopyHelp, expressionEngine));
    }

    public override async Task OnExecuteAsync(ClipboardArguments arguments, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(arguments.Input);

        if (!Enum.TryParse<CopyAction>(arguments.TargetKey, true, out var action))
        {
            action = CopyAction.Clipboard;
        }

        var textActions = TextActions.Resolve(arguments);

        foreach (var textAction in textActions)
        {
            await textActionInvoker.ExecuteAsync(textAction, arguments, cancellationToken);
        }

        var placeholders = (await keyValueRepository.GetAsync(null, null, cancellationToken)).ToList();

        if (!string.IsNullOrWhiteSpace(arguments.Parameters))
        {
            var parameters = KeyValuePairHelper.GetKeyValuePairs(arguments.Parameters.Split(' '), [.. applicationSettings.KeyValueSeparators]);

            if (parameters is not null && parameters.Any(x => x.HasValue))
            {
                placeholders.AddRange(parameters.Where(x => x.HasValue).Select(x => x!.Value));
            }
        }

        arguments.Input = await ReplacePlaceholdersAsync(arguments.Input, expressionEngine, placeholders.ToDictionary());

        await ioStream.Out.WriteLineAsync($"Copying {arguments.Input} to {action}");
        await copyActionInvoker.ExecuteAsync(action, arguments, cancellationToken);
        
    }
}