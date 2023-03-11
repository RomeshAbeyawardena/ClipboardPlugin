using Microsoft.Extensions.Configuration.CommandLine;
using Microsoft.Extensions.Configuration;

namespace ClipboardPlugin;

public partial class Program
{
    private static readonly ConfigurationBuilder cb = new();
    private static ApplicationSettings? applicationSettings;
    private static void WriteColouredText(string message, ConsoleColor consoleColor, params object[] args)
    {
        var original = Console.ForegroundColor;
        Console.ForegroundColor = consoleColor;
        Console.WriteLine(message, args);
        Console.ForegroundColor = original;
    }

    private static void WriteError(string message, params object[] args)
    {
        WriteColouredText($"ERROR: {message}", ConsoleColor.Red, args);
    }

    private static void WriteDebug(string message, params object[] args)
    {
#if DEBUG
        WriteColouredText($"DEBUG: {message}", ConsoleColor.DarkCyan, args);
#endif
    }

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
        WriteDebug("{0}", applicationSettings);
        var consoleConfiguration = cb.Add(new CommandLineConfigurationSource() { Args = args, SwitchMappings = applicationSettings.SwitchingProfile }).Build();
        return GetCommandLineArguments(consoleConfiguration);
    }

    public async static Task<int> Main(string[] args)
    {
        var commandLineArguments = SetupEnvironment(args);

        if (!DisplayVersion(commandLineArguments) && !DisplayHelp(commandLineArguments))
        {
            await CopyText(commandLineArguments);
        }

        return 0;
    }
}
