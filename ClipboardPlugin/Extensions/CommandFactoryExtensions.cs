using ClipboardPlugin.Contracts;

namespace ClipboardPlugin.Extensions
{
    internal static class CommandFactoryExtensions
    {
        internal static async Task Execute(this IEnumerable<ICommand> commands, CommandLineArguments arguments)
        {
            if (arguments.Async)
            {
                await Task.WhenAll(commands.Select(c => c.Execute(arguments)));
            }
            else
            {
                foreach (var command in commands)
                {
                    await command.Execute(arguments);
                }
            }
            //return Task.WhenAll(commands.Select(c => c.Execute(arguments)));
        }

        internal static async Task Execute(this ICommandFactory factory, CommandLineArguments arguments, string? name  = null)
        {
            await Execute(await factory.GetCommands(arguments, name), arguments);
        }
    }
}
