namespace ClipboardPlugin;

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
