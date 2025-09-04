namespace ClipboardPlugin.Actions;

public interface IAction<TAction, TActionSource>
    where TAction : Enum
{
    TActionSource? Source { get; set; }
    bool CanExecute(TAction action);
    Task<bool> CanExecuteAsync(TAction action, CancellationToken cancellationToken);
    Task ExecuteAsync(CancellationToken cancellationToken);
}
