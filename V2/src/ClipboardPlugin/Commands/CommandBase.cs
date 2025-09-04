using System.Reflection;

namespace ClipboardPlugin.Commands;

public abstract class CommandBase<TArguments>(string name, int? priority = null) : ICommand<TArguments>
{
    public string Name => name;
    public int Priority => priority.GetValueOrDefault();

    protected string ReplacePlaceholders(string value, IDictionary<string, string?>? externalPlaceholders = null)
    {
        var appName = Assembly.GetEntryAssembly()?.GetName();

        externalPlaceholders ??= new Dictionary<string,string?>();

        IReadOnlyDictionary<string, string?> allPlaceholders
            = new Dictionary<string, string?>(externalPlaceholders)
        {
            { "app", appName?.Name },
            { "version", appName?.Version?.ToString() }
        };

        var replacedValue = value;

        foreach(var items in allPlaceholders)
        {
            replacedValue = replacedValue.Replace($"{{{items.Key}}}", items.Value);
        }

        return replacedValue;
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
