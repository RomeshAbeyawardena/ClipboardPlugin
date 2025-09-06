namespace ClipboardPlugin.Actions;

public abstract class ActionInvokerBase<TAction, TActionSource>(IEnumerable<IAction<TAction, TActionSource>> actions)
    : IActionInvoker<TAction, TActionSource>
    where TAction : Enum
{
    public async Task ExecuteAsync(TAction action, TActionSource actionSource, CancellationToken cancellationToken)
    {
        foreach (var act in actions.OrderBy(x => x.Priority))
        {
            if (await act.CanExecuteAsync(action, cancellationToken))
            {
                act.Source = actionSource;
                await act.ExecuteAsync(cancellationToken);
            }
        }
    }
}
