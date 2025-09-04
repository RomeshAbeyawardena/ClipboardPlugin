namespace ClipboardPlugin;

public abstract class CommandBase<TArguments>(int? priority = null) : ICommand<TArguments>
{
    public int Priority => priority.GetValueOrDefault();

    public virtual bool CanExecute(TArguments arguments)
    {
        return true;
    }

    public virtual Task<bool> CanExecuteAsync(TArguments arguments, CancellationToken cancellationToken)
    {
        return Task.FromResult(CanExecute(arguments));
    }

    public abstract Task ExecuteAsync(TArguments arguments, CancellationToken cancellationToken);
}

public abstract class HelpContextCommandBase<TArguments>(int? priority = null) : CommandBase<TArguments>(priority)
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