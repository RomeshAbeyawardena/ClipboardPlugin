using ClipboardPlugin.Extensions;
using RST.Attributes;

namespace ClipboardPlugin.Commands;

[Register]
public class InputFromFileCommand : CommandBase
{
    public InputFromFileCommand(IServiceProvider serviceProvider) 
        : base(serviceProvider, "input", string.Empty, int.MinValue)
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

    protected override Task<bool> OnCanExecute(CommandLineArguments arguments, string? command = null)
    {
        return this.CalculateCanExecute(arguments, !string.IsNullOrWhiteSpace(arguments.Input),
            false);
    }
}
