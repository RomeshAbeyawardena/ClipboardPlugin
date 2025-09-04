namespace ClipboardPlugin.Actions;

public interface IAction<TAction>
    where TAction : Enum
{
    bool CanExecute(TAction action);
    Task<bool> CanExecuteAsync(TAction action, CancellationToken cancellationToken);
    Task ExecuteAsync(CancellationToken cancellationToken);
}
