using ClipboardPlugin.Abstractions.Expressions;
using Microsoft.Extensions.DependencyInjection;

namespace ClipboardPlugin.ExpressionEngine.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConfigurationExpressionEngine(this IServiceCollection services)
    {
        return services.AddSingleton<IPlaceholderScanner, PlaceholderScanner>();
    }
}
