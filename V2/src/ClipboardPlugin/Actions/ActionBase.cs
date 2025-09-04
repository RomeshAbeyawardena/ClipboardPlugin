namespace ClipboardPlugin.Actions;

public abstract class ActionBase<TAction, TActionSource> : IAction<TAction, TActionSource>
    where TAction : Enum
{
    public TActionSource? Source { get; set; }
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
