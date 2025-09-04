namespace ClipboardPlugin;

public interface ICommandParser<TArguments>
{
    Task ExecuteAsync(CancellationToken cancellationToken);
    Task ExecuteAsync(TArguments arguments, CancellationToken cancellationToken);
}
