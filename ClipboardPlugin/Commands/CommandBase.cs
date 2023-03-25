﻿using ClipboardPlugin.Contracts;
using Microsoft.Extensions.Logging;
using RST.DependencyInjection.Extensions;
using RST.DependencyInjection.Extensions.Attributes;

namespace ClipboardPlugin.Commands;

public abstract class CommandBase : EnableInjectionBase<InjectAttribute>, ICommand
{
    [Inject]
    protected ILogger? Logger { get; set; }

    protected abstract Task<bool> OnCanExecute(CommandLineArguments arguments, string? command = null);
    public CommandBase(IServiceProvider serviceProvider, string name, string? helpText = null,
        int? order = null)
        : base(serviceProvider)
    {
        Name = name;
        HelpText = helpText;
        Order = order.GetValueOrDefault();
    }

    public int Order { get; }
    public string Name { get; }
    public string? HelpText { get; }

    public async Task<bool> CanExecute(CommandLineArguments arguments, string? command = null)
    {
        ConfigureInjection();
        bool canExecute = (string.IsNullOrEmpty(command) || command.Equals(Name, StringComparison.InvariantCultureIgnoreCase)) && await OnCanExecute(arguments, command);

        if(canExecute)
        {
            var type = GetType();
            Logger!.LogDebug("Using {type}. Order: {Order}", type, CommandOrder.Describe(Order));
        }

        return canExecute;
    }

    public abstract Task Execute(CommandLineArguments arguments, string? command = null);
}
