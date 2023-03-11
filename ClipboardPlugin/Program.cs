using ClipboardPlugin.Contracts;
using ClipboardPlugin.Commands;
using ClipboardPlugin.Defaults;
using ClipboardPlugin.Extensions;

namespace ClipboardPlugin;

public partial class Program
{
    public async static Task<int> Main(string[] args)
    {

        factory = new CommandFactory(new ICommand[] { new CopyToClipboardCommand(
            consoleService = new ConsoleService()),
            new HelpCommand(versionService = new VersionService()),
            new VersionCommand(versionService)
        });

        await factory.Execute(SetupEnvironment(args));
        return 0;
    }
}