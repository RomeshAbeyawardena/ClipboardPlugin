namespace ClipboardPlugin.Actions;

public interface IActionInvoker<TAction>
    where TAction : Enum
{
    Task ExecuteAsync(TAction action, CancellationToken cancellationToken);
}