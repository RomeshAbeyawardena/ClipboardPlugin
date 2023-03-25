using Microsoft.Extensions.Configuration.CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using RST.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using ClipboardPlugin.Defaults;
using Microsoft.Extensions.Logging;

namespace ClipboardPlugin;

public partial class Program
{
    private static readonly ConfigurationBuilder cb = new();
    private static ApplicationSettings? applicationSettings;
    
    private static IServiceProvider? serviceProvider;

    private static ApplicationSettings GetApplicationSettings(IConfiguration configuration)
    {
        return new ApplicationSettings(configuration);
    }

    private static CommandLineArguments GetCommandLineArguments(IConfiguration configuration)
    {
        return new CommandLineArguments(configuration);
    }

    private static CommandLineArguments SetupEnvironment(string[] args, out IConfiguration appConfiguration)
    {
        appConfiguration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        applicationSettings = GetApplicationSettings(appConfiguration);
        
        var consoleConfiguration = cb.Add(new CommandLineConfigurationSource() { 
            Args = args, 
            SwitchMappings = applicationSettings.SwitchingProfile
        }).Build();

        return GetCommandLineArguments(consoleConfiguration);
    }

    private static void AddServices(string[] args)
    {
        IConfiguration appConfiguration;

        var services = new ServiceCollection()
            .AddSingleton(SetupEnvironment(args, out appConfiguration))
            .AddLogging(c => c
                .AddConfiguration(appConfiguration)
                .AddConsole())
            .AddSingleton<ILogger>(s => s.GetRequiredService<ILogger<Program>>());

        services
            .AddServicesWithRegisterAttribute(opt => { opt.ConfigureCoreServices = true; },
        Assembly.GetCallingAssembly());
        
        services.TryAdd(ServiceDescriptor.Describe(typeof(IFileProvider), typeof(SystemFileProvider), ServiceLifetime.Singleton));

        serviceProvider = services.BuildServiceProvider();
    }
}