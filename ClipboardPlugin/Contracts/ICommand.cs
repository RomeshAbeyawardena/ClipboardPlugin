﻿namespace ClipboardPlugin.Contracts;

public interface ICommand
{
    int Order { get; } 
    string Name { get; }
    string? HelpText { get; }
    Task<bool> CanExecute(CommandLineArguments arguments, string? command = null);
    Task Execute(CommandLineArguments arguments, string? command = null);
}
