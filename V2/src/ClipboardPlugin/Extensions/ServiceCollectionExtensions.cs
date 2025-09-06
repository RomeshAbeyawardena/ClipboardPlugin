using ClipboardPlugin.Actions;
using ClipboardPlugin.Actions.Copying;
using ClipboardPlugin.Actions.Text;
using ClipboardPlugin.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClipboardPlugin.Extensions;

public static class ServiceCollectionExtensions
{
    private static bool IsOfType(this Type type, Type targetType)
    {
        var interfaces = type.GetInterfaces().FirstOrDefault(i => i.IsGenericType);

        if (interfaces is null)
        {
            return false;
        }

        var res = interfaces.GetGenericTypeDefinition() == targetType;
        return res;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services.AddSingleton<TextCopy.IClipboard, TextCopy.Clipboard>();
    }

    public static IServiceCollection AddCommands(this IServiceCollection services)
    {
        return services
            .AddTransient<ICommandParser<ClipboardArguments>, ClipboardArgumentsCommandParser>()
            .Scan(x => x.FromAssemblyOf<ClipboardArguments>()
                .AddClasses(x => x.Where(x => x.IsOfType(typeof(ICommand<>))), false
                ).AsImplementedInterfaces().WithTransientLifetime())
            .AddTransient<IFileProvider, PhysicalFileProvider>()
            .AddTransient<IKeyValueRepository>(s =>
            {
                var fileProvider = s.GetRequiredService<IFileProvider>();
                var configuration = s.GetRequiredService<IConfiguration>();
                return new JsonFileKeyValueRepository(configuration["settings-file-name"] 
                    ?? throw new NullReferenceException("Key not found"), fileProvider);
            })
            .AddTransient<IActionInvoker<CopyAction, ClipboardArguments>, CopyActionInvoker>()
            .AddTransient<IActionInvoker<TextAction, ClipboardArguments>, TextActionInvoker>()
            .Scan(x => x.FromAssemblyOf<ClipboardArguments>()
                .AddClasses(x => x.Where(x => x.IsOfType(typeof(IAction<,>))), false
                ).AsImplementedInterfaces().WithTransientLifetime());
    }
}
