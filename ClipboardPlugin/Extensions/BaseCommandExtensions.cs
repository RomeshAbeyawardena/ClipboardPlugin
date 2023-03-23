using ClipboardPlugin.Contracts;

namespace ClipboardPlugin.Extensions;

public static class BaseCommandExtensions
{
    public static Task<bool> CalculateCanExecute(this ICommand command, CommandLineArguments arguments, bool canExecute)
    {
        return Task.FromResult(!arguments.Help.HasValue
            && !arguments.Version.HasValue
            && !string.IsNullOrWhiteSpace(arguments.Text)
            && canExecute);
    }
}
