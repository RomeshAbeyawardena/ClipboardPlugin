
using ClipboardPlugin.Repositories;
using TextCopy;

namespace ClipboardPlugin.Commands;

internal class DefineCommand(IKeyValueRepository keyValueRepository, IIoStream ioStream, IClipboard clipboard) 
    : HelpContextCommandBase<ClipboardArguments>(DISPLAY_NAME, 1)
{
    private static char? DetermineSeparator(string value, params char[] c)
    {
        char? currentSeparator;
        int index = 0;
        do
        {
            currentSeparator = c[index++];
            if (!value.Contains(currentSeparator.Value))
            {
                currentSeparator = null;
            }
        }
        while (index < c.Length && currentSeparator is null);
        return currentSeparator;
    }

    private static IEnumerable<(string, string?)?> GetKeyValuePairs(IEnumerable<string> values, params char[] c)
    {
        if (!values.Any())
        {
            return [];
        }

        var currentSeparator = DetermineSeparator(values.First(), c);

        if (currentSeparator is null)
        {
            return [];
        }

        return values.Select(x => GetKeyValuePair(x, currentSeparator));
    }

    private static (string, string?)? GetKeyValuePair(string value, char? currentSeparator)
    {
        if (!currentSeparator.HasValue)
        {
            return null;
        }

        var definition = value.Split(currentSeparator.Value);

        if (definition.Length == 2)
        {
            var key = definition[0];
            var val = definition[1];

            return (key, val);
        }

        return null;
    }

    private static (string, string?)? GetKeyValuePair(string value, params char[] c)
    {
        char? currentSeparator = DetermineSeparator(value, c);
        return GetKeyValuePair(value, currentSeparator);
    }

    public const string DISPLAY_NAME = "DEFINE";

    public override Task RenderContextHelpAsync(ClipboardArguments arguments, CancellationToken cancellationToken)
    {
        return base.RenderContextHelpAsync(arguments, cancellationToken);
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

        var keyValuePair = GetKeyValuePair(arguments.Define, ':', '=');

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
