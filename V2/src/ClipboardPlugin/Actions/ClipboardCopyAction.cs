
using TextCopy;

namespace ClipboardPlugin.Actions;

internal class ClipboardCopyAction(IClipboard clipboard) : ActionBase<CopyAction, ClipboardArguments>
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

internal class FileCopyAction(IIoStream ioStream) : ActionBase<CopyAction, ClipboardArguments>
{
    public override bool CanExecute(CopyAction action)
    {
        return action == CopyAction.File;
    }

    public override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        if (Source is null)
        {
            throw new NullReferenceException("Arguments property must not be null");
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(Source.Input, nameof(Source));
        ArgumentException.ThrowIfNullOrWhiteSpace(Source.Target, nameof(Source));

        await ioStream.Out.WriteLineAsync(Source.TargetParameter);

        await Task.CompletedTask;
    }
}