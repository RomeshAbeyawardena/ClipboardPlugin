using ClipboardPlugin.Contracts;
using Microsoft.Extensions.DependencyInjection;
using RST.DependencyInjection.Extensions;
using RST.DependencyInjection.Extensions.Attributes;
using RST.Extensions;

namespace ClipboardPlugin.Commands;

public abstract class BaseCommand : EnableInjectionBase<InjectAttribute>, ICommand
{
    protected abstract Task<bool> OnCanExecute(CommandLineArguments arguments, string? command = null);
    public BaseCommand(IServiceProvider serviceProvider, string name, string? helpText = null)
        : base(serviceProvider)
    {
        Name = name;
        HelpText = helpText;
    }

    public string Name { get; }
    public string? HelpText { get; }

    public async Task<bool> CanExecute(CommandLineArguments arguments, string? command = null)
    {
        ConfigureInjection();
        return (string.IsNullOrEmpty(command) || command.Equals(Name, StringComparison.InvariantCultureIgnoreCase)) && await OnCanExecute(arguments, command);
    }

    public abstract Task Execute(CommandLineArguments arguments, string? command = null);
}
