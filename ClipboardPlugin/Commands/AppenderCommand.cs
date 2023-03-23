using ClipboardPlugin.Extensions;

namespace ClipboardPlugin.Commands;

public class AppenderCommand : CommandBase
{
    public AppenderCommand(IServiceProvider serviceProvider)
        : base(serviceProvider, "append", string.Empty, CommandOrder.PROCESS_LOWER_PRIORITY_COMMAND)
    {

    }

    public override Task Execute(CommandLineArguments arguments, string? command = null)
    {
        if (!string.IsNullOrWhiteSpace(arguments.PrependValue))
        {
            arguments.Text = $"{arguments.PrependValue}{arguments.Text}";
        }
        if (!string.IsNullOrWhiteSpace(arguments.AppendValue))
        {
            arguments.Text = $"{arguments.Text}{arguments.AppendValue}";
        }

        return Task.CompletedTask;
    }

    protected override Task<bool> OnCanExecute(CommandLineArguments arguments, string? command = null)
    {   
        return this.CalculateCanExecute(arguments, 
            !string.IsNullOrWhiteSpace(arguments.PrependValue) ||
            !string.IsNullOrWhiteSpace(arguments.AppendValue),
            string.IsNullOrWhiteSpace(arguments.Input));
    }
}
