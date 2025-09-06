
namespace ClipboardPlugin.Commands;

internal class DefineCommand(IKeyValueRepository keyValueRepository, IIoStream ioStream) : HelpContextCommandBase<ClipboardArguments>(DISPLAY_NAME, 1)
{
    private static (string, string?)? GetKeyValuePair(string value, params char[] c)
    {
        char? currentSeparator;
        int index = 0;
        do
        {
            currentSeparator = c[index++];
        }
        while (index < c.Length && currentSeparator is null);

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
            }

            await ioStream.Error.WriteLineAsync("Not found");
            
        }
    }
}
