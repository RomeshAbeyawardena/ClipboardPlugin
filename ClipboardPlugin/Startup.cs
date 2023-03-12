using ClipboardPlugin.Contracts;
using ClipboardPlugin.Extensions;
using RST.Attributes;

namespace ClipboardPlugin;

[Register]
public class Startup : IStartup
{
    private readonly ICommandFactory commandFactory;
    private readonly CommandLineArguments commandLineArguments;

    public Startup(ICommandFactory commandFactory, CommandLineArguments commandLineArguments)
    {
        this.commandFactory = commandFactory;
        this.commandLineArguments = commandLineArguments;
    }

    public async Task RunAsync()
    {
        await commandFactory.Execute(commandLineArguments);
    }
}
