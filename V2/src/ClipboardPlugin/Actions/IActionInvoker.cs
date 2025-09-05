namespace ClipboardPlugin.Actions;

public interface IActionInvoker<TAction, TActionSource>
    where TAction : Enum
{
    Task ExecuteAsync(TAction action, TActionSource actionSource, CancellationToken cancellationToken);
}
