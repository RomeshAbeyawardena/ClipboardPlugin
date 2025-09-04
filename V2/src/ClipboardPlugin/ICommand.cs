namespace ClipboardPlugin;

public interface ICommand<TArguments>
{
    bool CanExecute(TArguments arguments);
    Task<bool> CanExecuteAsync(TArguments arguments, CancellationToken cancellationToken);
    Task ExecuteAsync(TArguments arguments, CancellationToken cancellationToken);
}
