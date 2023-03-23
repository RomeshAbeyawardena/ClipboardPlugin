using ClipboardPlugin.Extensions;

namespace ClipboardPlugin.Commands;

public class AppenderCommand : CommandBase
{
    public AppenderCommand(IServiceProvider serviceProvider)
        : base(serviceProvider, "append", string.Empty, 50)
    {

    }

    public override Task Execute(CommandLineArguments arguments, string? command = null)
    {
        if (!string.IsNullOrWhiteSpace(arguments.PrependerValue))
        {
            arguments.Text = $"{arguments.PrependerValue}{arguments.Text}";
        }
        if (!string.IsNullOrWhiteSpace(arguments.AppenderValue))
        {
            arguments.Text = $"{arguments.Text}{arguments.AppenderValue}";
        }

        return Task.CompletedTask;
    }

    protected override Task<bool> OnCanExecute(CommandLineArguments arguments, string? command = null)
    {   
        return this.CalculateCanExecute(arguments, 
            !string.IsNullOrWhiteSpace(arguments.PrependerValue) ||
            !string.IsNullOrWhiteSpace(arguments.AppenderValue));
    }
}
