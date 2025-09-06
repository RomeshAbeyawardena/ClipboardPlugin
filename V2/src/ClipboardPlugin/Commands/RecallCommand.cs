
using ClipboardPlugin.Repositories;
using System.Collections.Generic;
using System.IO;
using TextCopy;

namespace ClipboardPlugin.Commands;

internal class RecallCommand(IKeyValueRepository keyValueRepository, IIoStream ioStream, IClipboard clipboard)
    : HelpContextCommandBase<ClipboardArguments>(DISPLAY_NAME, 1)
{
    public const string DISPLAY_NAME = "RECALL";
    public override bool CanExecute(ClipboardArguments arguments)
    {
        return !string.IsNullOrWhiteSpace(arguments.Recall);
    }

    public override Task RenderContextHelpAsync(ClipboardArguments arguments, CancellationToken cancellationToken)
    {
        return base.RenderContextHelpAsync(arguments, cancellationToken);
    }

    public override async Task OnExecuteAsync(ClipboardArguments arguments, CancellationToken cancellationToken)
    {
        if(string.IsNullOrWhiteSpace(arguments.Recall))
        {
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
