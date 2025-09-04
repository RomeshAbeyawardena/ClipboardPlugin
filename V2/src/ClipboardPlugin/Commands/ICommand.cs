namespace ClipboardPlugin.Commands;

public interface ICommand<TArguments>
{
    int Priority { get; }
    bool CanExecute(TArguments arguments);
    Task<bool> CanExecuteAsync(TArguments arguments, CancellationToken cancellationToken);
    Task ExecuteAsync(TArguments arguments, CancellationToken cancellationToken);
}
