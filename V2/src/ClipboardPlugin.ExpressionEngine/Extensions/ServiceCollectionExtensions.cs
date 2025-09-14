using ClipboardPlugin.Abstractions.Expressions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Globalization;

namespace ClipboardPlugin.ExpressionEngine.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConfigurationExpressionEngine(this IServiceCollection services)
    {
        services.TryAddSingleton(TimeProvider.System);
        services.TryAddSingleton(CultureInfo.CurrentUICulture);
        return services
            .AddSingleton<IPlaceholderScanner, PlaceholderScanner>()
            .AddSingleton<IExpressionEngine, ConfigurationExpressionEngine>();
    }
}
