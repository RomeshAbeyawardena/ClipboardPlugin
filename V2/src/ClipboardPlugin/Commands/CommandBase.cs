namespace ClipboardPlugin.Commands;

public abstract class CommandBase<TArguments>(string name, int? priority = null) : ICommand<TArguments>
{
    public string Name => name;
    public int Priority => priority.GetValueOrDefault();

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
