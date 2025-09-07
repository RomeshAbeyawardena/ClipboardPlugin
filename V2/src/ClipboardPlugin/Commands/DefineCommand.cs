
using ClipboardPlugin.Extensions;
using ClipboardPlugin.Repositories;
using TextCopy;

namespace ClipboardPlugin.Commands;

internal class DefineCommand(IKeyValueRepository keyValueRepository, IIoStream ioStream, IClipboard clipboard, IApplicationSettings applicationSettings) 
    : HelpContextCommandBase<ClipboardArguments>(DISPLAY_NAME, 1)
{
    private static (string, string?)? GetKeyValuePair(string value, params char[] c)
    {
        char? currentSeparator = KeyValuePairHelper.DetermineSeparator(value, c);
        return KeyValuePairHelper.GetKeyValuePair(value, currentSeparator);
    }

    public const string DISPLAY_NAME = "define";

    public override async Task RenderContextHelpAsync(ClipboardArguments arguments, CancellationToken cancellationToken)
    {
        await ioStream.Out.WriteLineAsync(Properties.Resources.DefineHelp);
    }

    public override bool CanExecute(ClipboardArguments arguments)
    {
        return !string.IsNullOrWhiteSpace(arguments.Define);
    }

    public override async Task OnExecuteAsync(ClipboardArguments arguments, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(arguments.Define))
        {
            return;
        }

        var keyValuePair = GetKeyValuePair(arguments.Define, [.. applicationSettings.KeyValueSeparators]);

        if (keyValuePair.HasValue)
        {
            await keyValueRepository.UpsertAsync(keyValuePair.Value, cancellationToken);
            await keyValueRepository.SaveChangesAsync(cancellationToken);
        }
        else
        {
            keyValuePair = await keyValueRepository.GetAsync(arguments.Define, cancellationToken);

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
}
