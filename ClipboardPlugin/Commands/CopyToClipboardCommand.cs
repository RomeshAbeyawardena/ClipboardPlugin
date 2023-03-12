﻿using ClipboardPlugin.Contracts;
using ClipboardPlugin.Extensions;
using ClipboardPlugin.Properties;
using RST.Attributes;

namespace ClipboardPlugin.Commands;

[Register]
public class CopyToClipboardCommand : BaseCommand
{
    private readonly IConsoleService consoleService;

    protected override async Task<bool> OnCanExecute(CommandLineArguments arguments, string? command = null)
    {
        await Task.CompletedTask;
        return !arguments.Help.HasValue
            && !arguments.Version.HasValue
            && string.IsNullOrWhiteSpace(arguments.Output)
            && !string.IsNullOrWhiteSpace(arguments.Text);
    }

    public CopyToClipboardCommand(IConsoleService consoleService, IServiceProvider serviceProvider)
        : base(serviceProvider, "copy", Resources.HelpText_Command_CopyToClipboard)
    {
        this.consoleService = consoleService;
    }

    public override async Task Execute(CommandLineArguments arguments, string? commandName = null)
    {
        await Task.CompletedTask;
        consoleService.WriteDebug("{0}", arguments);
        var currentClipboard = await TextCopy.ClipboardService.GetTextAsync();
        if (!string.IsNullOrWhiteSpace(arguments!.Text))
        {
            var textToCopy = arguments.HandleTextProcessing();

            if (!string.IsNullOrWhiteSpace(textToCopy) && !textToCopy.Equals(currentClipboard))
                await TextCopy.ClipboardService.SetTextAsync(textToCopy);
            else
                consoleService.WriteError(Resources.ErrorMessage_ContentIsNullOrEmptyOrAlreadyCopied);
        }
        else
            consoleService.WriteError(Properties.Resources.ErrorMessage_ContentIsNullOrEmpty);
    }
}
