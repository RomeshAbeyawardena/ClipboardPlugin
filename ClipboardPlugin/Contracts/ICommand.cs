namespace ClipboardPlugin.Contracts;

internal interface ICommand
{
    string Name { get; }
    string? HelpText { get; }
    Task<bool> CanExecute(CommandLineArguments arguments);
    Task Execute(CommandLineArguments arguments);
}
