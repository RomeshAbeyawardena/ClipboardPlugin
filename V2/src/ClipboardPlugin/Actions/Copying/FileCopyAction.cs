namespace ClipboardPlugin.Actions.Copying;

internal class FileCopyAction(IIoStream ioStream) : ActionBase<CopyAction, ClipboardArguments>(DEFAULT_PRIORITY)
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