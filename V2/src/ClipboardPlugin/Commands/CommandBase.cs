using ClipboardPlugin.Abstractions.Expressions;
using NCalc;
using System.Reflection;

namespace ClipboardPlugin.Commands;

public abstract class CommandBase<TArguments>(string name, int? priority = null) : ICommand<TArguments>
{
    public string Name => name;
    public int Priority => priority.GetValueOrDefault();

    protected void ExtendExpressionEngine(IExpressionEngine expressionEngine, Action<ExpressionContextBase> extend)
    {
        var appName = Assembly.GetEntryAssembly()?.GetName();
        expressionEngine.Extend(e =>
        {
            e.StaticParameters ??= new Dictionary<string, object?>();
            e.StaticParameters.TryAdd("app", appName?.Name);
            e.StaticParameters.TryAdd("version", appName?.Version?.ToString());
            extend(e);
        });
    }

    protected async Task<string> ReplacePlaceholdersAsync(string value, IExpressionEngine expressionEngine, IDictionary<string, string?>? externalPlaceholders = null)
    {
        ExtendExpressionEngine(expressionEngine, e => { 
            e.StaticParameters ??= new Dictionary<string, object?>();
            var placeholders = externalPlaceholders;
            if (placeholders is not null)
            {
                foreach (var (key, val) in placeholders)
                {
                    if (!e.StaticParameters.TryAdd(key, val))
                    {
                        e.StaticParameters[key] = val;
                    }
                    
                }
            }
        });

        return await expressionEngine.ResolveAsync(value);
    }

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
