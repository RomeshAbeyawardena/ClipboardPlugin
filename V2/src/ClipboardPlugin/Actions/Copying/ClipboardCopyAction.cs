using TextCopy;

namespace ClipboardPlugin.Actions.Copying;

internal class ClipboardCopyAction(IClipboard clipboard) : ActionBase<CopyAction, ClipboardArguments>(DEFAULT_PRIORITY)
{
    public override bool CanExecute(CopyAction action)
    {
        return action == CopyAction.Clipboard;
    }

    public override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        if (Source is null)
        {
            throw new NullReferenceException("Arguments property must not be null");
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(Source.Input, nameof(Source));

        await clipboard.SetTextAsync(Source.Input, cancellationToken);
    }
}
