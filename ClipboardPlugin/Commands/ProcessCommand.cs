using ClipboardPlugin.Extensions;
using RST.Attributes;

namespace ClipboardPlugin.Commands;

[Register]
public class ProcessCommand : CommandBase
{
    public ProcessCommand(IServiceProvider serviceProvider)
        : base(serviceProvider, "process", string.Empty, CommandOrder.PROCESS_MEDIUM_PRIORITY_COMMAND)
    {

    }

    public override Task Execute(CommandLineArguments arguments, string? command = null)
    {
        var textToCopy = arguments.Text;
        if (!string.IsNullOrEmpty(arguments.Text) && !string.IsNullOrEmpty(arguments.SplitString))
        {
            var splitString = arguments.Text.Split(arguments.SplitString);
            if (arguments.Index.HasValue)
            {
                if (arguments.Index != -1)
                {
                    textToCopy = splitString.ElementAtOrDefault(arguments.Index.Value);
                }
                else
                    textToCopy = splitString.LastOrDefault();
            }
            else
                textToCopy = string.Join(",", splitString);
        }
        
        arguments.Text = textToCopy;
        return Task.CompletedTask;
    }

    protected override Task<bool> OnCanExecute(CommandLineArguments arguments, string? command = null)
    {
        return this.CalculateCanExecute(arguments, arguments.Process, 
            string.IsNullOrWhiteSpace(arguments.Input));
    }
}
