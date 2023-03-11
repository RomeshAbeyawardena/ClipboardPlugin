using ClipboardPlugin.Contracts;

namespace ClipboardPlugin.Commands;

internal class VersionCommand : ICommand
{
    private readonly IVersionService versionService;

    public VersionCommand(IVersionService versionService)
    {
        Name = "version";
        this.versionService = versionService;
    }
    public string Name { get; }
    public string? HelpText { get; }

    public async Task<bool> CanExecute(CommandLineArguments arguments)
    {
        await Task.CompletedTask;
        return !arguments.Help.HasValue && arguments.Version.HasValue && arguments.Version.Value;
    }

    public Task Execute(CommandLineArguments arguments)
    {
        Console.WriteLine(Properties.Resources.HelpText_Version.Replace("{version}", versionService.GetVersion()!.ToString()));
        return Task.CompletedTask;
    }
}
