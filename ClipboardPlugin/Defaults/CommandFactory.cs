using ClipboardPlugin.Contracts;
using RST.Attributes;

namespace ClipboardPlugin.Defaults;

[Register]
public class CommandFactory : ICommandFactory
{
    private readonly IEnumerable<ICommand> commands;

    public CommandFactory(IEnumerable<ICommand> commands)
    {
        this.commands = commands;
    }

    public IEnumerable<ICommand> Commands => commands;

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
