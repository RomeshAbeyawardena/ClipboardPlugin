
using System.Text.RegularExpressions;

namespace ClipboardPlugin.Actions.Text;

internal class TextReplaceAction : ActionBase<TextAction, ClipboardArguments>
{
    public override bool CanExecute(TextAction action)
    {
        return action == TextAction.Replace;
    }

    public override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var arguments = base.Source;

        if (arguments is not null 
            && !string.IsNullOrWhiteSpace(arguments.Input) 
            && !string.IsNullOrWhiteSpace(arguments.Find))
        {
            arguments.Input = arguments.Regex
                ? Regex.Replace(arguments.Input, arguments.Find, arguments.Replace ?? string.Empty)
                : arguments.Input.Replace(arguments.Find, arguments.Replace);
        }

        return Task.CompletedTask;
    }
}
