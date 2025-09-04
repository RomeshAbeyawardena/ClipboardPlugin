
using TextCopy;

namespace ClipboardPlugin.Actions;

public class ClipboardCopyAction(IClipboard clipboard) : ActionBase<CopyAction>
{
    public ClipboardArguments? Arguments { get; set; }

    public override bool CanExecute(CopyAction action)
    {
        return action == CopyAction.Clipboard;
    }

    public override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        if (Arguments is null)
        {
            throw new NullReferenceException("Arguments property must not be null");
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(Arguments.Source, nameof(Arguments));

        await clipboard.SetTextAsync(Arguments.Source, cancellationToken);
    }
}