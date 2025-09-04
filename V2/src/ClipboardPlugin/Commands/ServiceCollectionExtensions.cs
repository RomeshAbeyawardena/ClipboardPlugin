using Microsoft.Extensions.DependencyInjection;

namespace ClipboardPlugin.Commands;

public static class ServiceCollectionExtensions
{
    private static bool IsOfType(Type type)
    {
        var interfaces = type.GetInterfaces().FirstOrDefault(i => i.IsGenericType);

        if (interfaces is null)
        {
            return false;
        }

        var res = interfaces.GetGenericTypeDefinition() == typeof(ICommand<>);
        return res;
    }

    public static IServiceCollection AddCommands(this IServiceCollection services)
    {
        return services
            .AddTransient<ICommandParser<ClipboardArguments>, ClipboardArgumentsCommandParser>()
            .Scan(x => x.FromAssemblyOf<ClipboardArguments>()
                .AddClasses(x => x.Where(IsOfType), false
                ).AsImplementedInterfaces());
    }
}
