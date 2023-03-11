using ClipboardPlugin.Contracts;

namespace ClipboardPlugin.Extensions
{
    internal static class CommandFactoryExtensions
    {
        internal static Task Execute(this IEnumerable<ICommand> commands, CommandLineArguments arguments)
        {
            return Task.WhenAll(commands.Select(c => c.Execute(arguments)));
        }

        internal static async Task Execute(this ICommandFactory factory, CommandLineArguments arguments, string? name  = null)
        {
            await Execute(await factory.GetCommands(arguments, name), arguments);
        }
    }
}
