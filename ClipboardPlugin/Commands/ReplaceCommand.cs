using RST.Attributes;

namespace ClipboardPlugin.Commands;

[Register]
public class ReplaceCommand : BaseCommand
{
    public ReplaceCommand(IServiceProvider serviceProvider, string name, string? helpText = null) : base(serviceProvider, name, helpText)
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

    protected override async Task<bool> OnCanExecute(CommandLineArguments arguments, string? command = null)
    {
        await Task.CompletedTask;
        return !string.IsNullOrWhiteSpace(arguments.SearchString)
            && !string.IsNullOrWhiteSpace(arguments.ReplacementString);
    }
}
