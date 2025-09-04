namespace ClipboardPlugin;

public class ClipboardArgumentsCommandParser(IEnumerable<ICommand<ClipboardArguments>> commands, ClipboardArguments clipboardArguments) : ICommandParser<ClipboardArguments>
{
    public Task ExecuteAsync(CancellationToken cancellationToken)
    {
        return ExecuteAsync(clipboardArguments, cancellationToken);
    }

    public async Task ExecuteAsync(ClipboardArguments arguments, CancellationToken cancellationToken)
    {
        foreach (var command in commands)
        {
            if (await command.CanExecuteAsync(arguments, cancellationToken))
            {
                await command.ExecuteAsync(arguments, cancellationToken);
            }
        }
    }
}
