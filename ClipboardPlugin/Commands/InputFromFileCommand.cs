using RST.Attributes;

namespace ClipboardPlugin.Commands;

[Register]
public class InputFromFileCommand : BaseCommand
{
    public InputFromFileCommand(IServiceProvider serviceProvider, string name, string? helpText = null) : base(serviceProvider, name, helpText, int.MinValue)
    {
    }

    public override async Task Execute(CommandLineArguments arguments, string? command = null)
    {
        if (!string.IsNullOrWhiteSpace(arguments.Input) 
            && File.Exists(arguments.Input))
        {
            arguments.Text = await File.ReadAllTextAsync(arguments.Input);
        }
    }

    protected override async Task<bool> OnCanExecute(CommandLineArguments arguments, string? command = null)
    {
        await Task.CompletedTask;
        return !string.IsNullOrWhiteSpace(arguments.Input);
    }
}
