namespace ClipboardPlugin;

public interface ICommand<TArguments>
{
    bool CanExecute(TArguments arguments);
    Task<bool> CanExecuteAsync(TArguments arguments, CancellationToken cancellationToken);
    Task ExecuteAsync(TArguments arguments, CancellationToken cancellationToken);
}

public abstract class CommandBase<TArguments> : ICommand<TArguments>
{
    public virtual bool CanExecute(TArguments arguments)
    {
        return true;
    }

    public virtual Task<bool> CanExecuteAsync(TArguments arguments, CancellationToken cancellationToken)
    {
        return Task.FromResult(CanExecute(arguments));
    }

    public abstract Task ExecuteAsync(TArguments arguments, CancellationToken cancellationToken);
}

public interface ICommandParser<TArguments>
{
    Task ExecuteAsync(TArguments arguments, CancellationToken cancellationToken);
}

public class ClipboardArgumentsCommandParser(IEnumerable<ICommand<ClipboardArguments>> commands) : ICommandParser<ClipboardArguments>
{
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
