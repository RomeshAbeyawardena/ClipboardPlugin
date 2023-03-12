using ClipboardPlugin.Contracts;
using ClipboardPlugin.Properties;
using RST.Attributes;

namespace ClipboardPlugin.Commands;

[Register]
public class VersionCommand : BaseCommand
{
    private readonly IVersionService versionService;
    protected override async Task<bool> OnCanExecute(CommandLineArguments arguments, string? commandName = null)
    {
        await Task.CompletedTask;
        return !arguments.Help.HasValue && arguments.Version.HasValue && arguments.Version.Value;
    }

    public VersionCommand(IVersionService versionService, IServiceProvider serviceProvider)
        : base(serviceProvider, "version", Resources.HelpText_Command_Version)
    {
        this.versionService = versionService;
    }

    public override Task Execute(CommandLineArguments arguments, string? commandName = null)
    {
        Console.WriteLine(Properties.Resources.HelpText_Version.Replace("{version}", versionService.GetVersion()!.ToString()));
        return Task.CompletedTask;
    }
}
