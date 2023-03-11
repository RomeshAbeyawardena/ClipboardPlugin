namespace ClipboardPlugin.Contracts;

internal interface ICommandFactory
{
    Task<IEnumerable<ICommand>> GetCommands(CommandLineArguments arguments, string? commandName = null);
}
