namespace ClipboardPlugin.Contracts;

public interface ICommandFactory
{
    IEnumerable<ICommand> Commands { get; }
    Task<IEnumerable<ICommand>> GetCommands(CommandLineArguments arguments, string? commandName = null);
}
