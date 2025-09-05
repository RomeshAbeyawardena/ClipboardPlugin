
namespace ClipboardPlugin.Actions.Text
{
    internal class ExtractFileNameAction : ActionBase<TextAction, ClipboardArguments>
    {
        public override bool CanExecute(TextAction action)
        {
            return action == TextAction.ExtractFilename;
        }
        public override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            if (Source is null)
            {
                return Task.CompletedTask;
            }

            Source.Input = Path.GetFileName(Source.Input);
            return Task.CompletedTask;
        }
    }
}
