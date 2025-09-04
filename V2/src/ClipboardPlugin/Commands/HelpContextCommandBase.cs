namespace ClipboardPlugin.Commands;

public abstract class HelpContextCommandBase<TArguments>(string name, int? priority = null) : CommandBase<TArguments>(name, priority)
    where TArguments : class, IHelpContextArgument
{
    public virtual void RenderContextHelp(TArguments arguments)
    {

    }

    public virtual Task RenderContextHelpAsync(TArguments arguments, CancellationToken cancellationToken)
    {
        RenderContextHelp(arguments);
        return Task.CompletedTask;
    }

    public abstract Task OnExecuteAsync(TArguments arguments, CancellationToken cancellationToken);

    public override Task ExecuteAsync(TArguments arguments, CancellationToken cancellationToken)
    {
        if (arguments.Help)
        {
            return RenderContextHelpAsync(arguments, cancellationToken);
        }

        return OnExecuteAsync(arguments, cancellationToken);
    }
}