using ClipboardPlugin.Contracts;
using ClipboardPlugin.Properties;
using RST.Attributes;
using RST.DependencyInjection.Extensions.Attributes;

namespace ClipboardPlugin.Commands;

[Register]
public class HelpCommand : BaseCommand
{
    private readonly IVersionService versionService;
    [Inject]
    private ICommandFactory? commandFactory { get; set; }

    protected override async Task<bool> OnCanExecute(CommandLineArguments arguments, string? command = null)
    {
        await Task.CompletedTask;
        return arguments.Help.HasValue && arguments.Help.Value;
    }

    public HelpCommand(IServiceProvider serviceProvider, IVersionService versionService)
        : base(serviceProvider, "help", Resources.HelpText_Command_Help)
    {
        this.versionService = versionService;
        //this.commandFactory = commandFactory;
    }

    public override async Task Execute(CommandLineArguments arguments, string? command = null)
    {
        if (!string.IsNullOrWhiteSpace(arguments.HelpContext))
        {
           var foundCommand = commandFactory.Commands.FirstOrDefault(c=> c.Name.Equals(arguments.HelpContext, StringComparison.InvariantCultureIgnoreCase));

            if(foundCommand != null && !string.IsNullOrWhiteSpace(foundCommand.HelpText))
            {
                Console.WriteLine("{0}\r\n{1}", 
                    Resources.HelpText_Version.Replace("{version}", 
                    versionService.GetVersion().ToString()), foundCommand.HelpText);
                return;
            }
        }

        Console.WriteLine(Resources.HelpText.Replace("{version}", 
            versionService.GetVersion()!.ToString()));
        await Task.CompletedTask;
    }
}
