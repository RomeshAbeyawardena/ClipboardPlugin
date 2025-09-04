namespace ClipboardPlugin.Actions;

public abstract class ActionBase<TAction> : IAction<TAction>
    where TAction : Enum
{
    public virtual bool CanExecute(TAction action)
    {
        return true;
    }

    public virtual Task<bool> CanExecuteAsync(TAction action, CancellationToken cancellationToken)
    {
        return Task.FromResult(CanExecute(action));
    }

    public abstract Task ExecuteAsync(CancellationToken cancellationToken);
}
