using Microsoft.Extensions.Configuration.CommandLine;
using Microsoft.Extensions.Configuration;
using System.Net.NetworkInformation;
using ClipboardPlugin.Defaults;
using ClipboardPlugin.Contracts;
using ClipboardPlugin.Commands;
using ClipboardPlugin.Extensions;

namespace ClipboardPlugin;

public partial class Program
{
    private static readonly ConfigurationBuilder cb = new();
    private static ApplicationSettings? applicationSettings;
    private static CommandFactory? factory;
    private static ConsoleService consoleService;
    private static VersionService versionService;

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
}
