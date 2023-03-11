using ClipboardPlugin.Contracts;

namespace ClipboardPlugin.Defaults;

internal class CommandFactory : ICommandFactory
{
    private readonly IEnumerable<ICommand> commands;

    public CommandFactory(IEnumerable<ICommand> commands)
    {
        this.commands = commands;
    }

    public async Task<IEnumerable<ICommand>> GetCommands(CommandLineArguments arguments,
        string? commandName = null)
    {
        var commandList = new List<ICommand>();
        foreach(var command in commands)
        {
            if(await command.CanExecute(arguments) 
                && (string.IsNullOrWhiteSpace(commandName) || command.Name.Equals(commandName, StringComparison.InvariantCultureIgnoreCase)))
            {
                commandList.Add(command);
            }
        }

        return commandList;
    }
}
