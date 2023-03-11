using ClipboardPlugin.Contracts;

namespace ClipboardPlugin.Commands;

internal class HelpCommand : ICommand
{
    private readonly IVersionService versionService;

    public HelpCommand(IVersionService versionService)
    {
        Name = "help";
        this.versionService = versionService;
    }

    public string Name { get; }
    public string? HelpText { get; }

    public async Task<bool> CanExecute(CommandLineArguments arguments)
    {
        await Task.CompletedTask;
        return arguments.Help.HasValue && arguments.Help.Value;
    }

    public async Task Execute(CommandLineArguments arguments)
    {
        Console.WriteLine(Properties.Resources.HelpText.Replace("{version}", 
            versionService.GetVersion()!.ToString()));
        await Task.CompletedTask;
    }
}
