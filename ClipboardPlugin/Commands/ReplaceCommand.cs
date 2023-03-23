using ClipboardPlugin.Extensions;
using RST.Attributes;

namespace ClipboardPlugin.Commands;

[Register]
public class ReplaceCommand : CommandBase
{
    public ReplaceCommand(IServiceProvider serviceProvider) 
        : base(serviceProvider, "replace", string.Empty)
    {
    }

    public override Task Execute(CommandLineArguments arguments, string? command = null)
    {
        if(!string.IsNullOrWhiteSpace(arguments.SearchString)
            && !string.IsNullOrWhiteSpace(arguments.ReplacementString)
            && !string.IsNullOrWhiteSpace(arguments.Text))
        {
            arguments.Text = arguments.Text.Replace(arguments.SearchString, arguments.ReplacementString);
        }

        return Task.CompletedTask;
    }

    protected override Task<bool> OnCanExecute(CommandLineArguments arguments, string? command = null)
    {
        return this.CalculateCanExecute(arguments,
            !string.IsNullOrWhiteSpace(arguments.SearchString)
            && !string.IsNullOrWhiteSpace(arguments.ReplacementString));
    }
}
