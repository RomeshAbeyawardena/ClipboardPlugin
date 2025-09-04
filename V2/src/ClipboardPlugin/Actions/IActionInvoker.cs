namespace ClipboardPlugin.Actions;

public interface IActionInvoker<TAction, TActionSource>
    where TAction : Enum
{
    Task ExecuteAsync(TAction action, TActionSource actionSource, CancellationToken cancellationToken);
}

public class CopyActionInvoker(IEnumerable<IAction<CopyAction, ClipboardArguments>> actions) : IActionInvoker<CopyAction, ClipboardArguments>
{
    public async Task ExecuteAsync(CopyAction action, ClipboardArguments actionSource, CancellationToken cancellationToken)
    {
        foreach(var act in actions)
        {
            if (await act.CanExecuteAsync(action, cancellationToken))
            {
                act.Source = actionSource;
                await act.ExecuteAsync(cancellationToken);
            }
        }
    }
}