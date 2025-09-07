
using ClipboardPlugin.Properties;
using ClipboardPlugin.Repositories;
using TextCopy;

namespace ClipboardPlugin.Commands;

internal class RecallCommand(IKeyValueRepository keyValueRepository, IIoStream ioStream, IClipboard clipboard)
    : HelpContextCommandBase<ClipboardArguments>(DISPLAY_NAME, 1)
{
    public const string DISPLAY_NAME = "recall";
    public override bool CanExecute(ClipboardArguments arguments)
    {
        return arguments.List.HasValue || !string.IsNullOrWhiteSpace(arguments.Recall);
    }

    public override Task RenderContextHelpAsync(ClipboardArguments arguments, CancellationToken cancellationToken)
    {
        return ioStream.Out.WriteLineAsync(Resources.RecallHelp);
    }

    public override async Task OnExecuteAsync(ClipboardArguments arguments, CancellationToken cancellationToken)
    {
        if(string.IsNullOrWhiteSpace(arguments.Recall))
        {
            return;
        }

        if (arguments.List.HasValue)
        {
            foreach (var (key, value) in await keyValueRepository.GetAsync(arguments.Recall.Replace("*", string.Empty), 
                arguments.List.Value, cancellationToken))
            {
                await ioStream.Out.WriteLineAsync($"{key}\t{value}");
            }

            return;
        }

        var keyValuePair = await keyValueRepository.GetAsync(arguments.Recall, cancellationToken);

        if (keyValuePair.HasValue)
        {
            var (key, value) = keyValuePair.Value;
            await ioStream.Out.WriteLineAsync($"{key}: {value}");

            if (value is not null)
            {
                await clipboard.SetTextAsync(value, cancellationToken);
            }
        }
        else
        {
            await ioStream.Error.WriteLineAsync("Not found");
        }
    }
}
