using Microsoft.Extensions.Configuration.CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RST.Extensions;
using System.Reflection;
using RST.DependencyInjection.Extensions;

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

    private static CommandLineArguments SetupEnvironment(string[] args)
    {
        var appSettingsConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        applicationSettings = GetApplicationSettings(appSettingsConfig);
        
        var consoleConfiguration = cb.Add(new CommandLineConfigurationSource() { 
            Args = args, 
            SwitchMappings = applicationSettings.SwitchingProfile
        }).Build();

        return GetCommandLineArguments(consoleConfiguration);
    }

    private static void AddServices(string[] args)
    {
        var services = new ServiceCollection()
            .AddSingleton(SetupEnvironment(args));
        services
            .AddServicesWithRegisterAttribute(opt => { opt.ConfigureCoreServices = true; },
        Assembly.GetCallingAssembly());

        serviceProvider = services.BuildServiceProvider();
    }
}