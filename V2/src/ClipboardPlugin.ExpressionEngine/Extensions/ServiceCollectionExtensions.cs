using ClipboardPlugin.Abstractions.Expressions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ClipboardPlugin.ExpressionEngine.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConfigurationExpressionEngine(this IServiceCollection services)
    {
        services.TryAddSingleton(TimeProvider.System);

        return services
            .AddSingleton<IPlaceholderScanner, PlaceholderScanner>()
            .AddSingleton<IExpressionEngine, ConfigurationExpressionEngine>();
    }
}
