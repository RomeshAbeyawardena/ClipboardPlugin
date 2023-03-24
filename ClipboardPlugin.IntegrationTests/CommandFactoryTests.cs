using ClipboardPlugin.Commands;
using ClipboardPlugin.Contracts;
using ClipboardPlugin.Defaults;
using Microsoft.Extensions.FileProviders;
using Moq;

namespace ClipboardPlugin.IntegrationTests;

public class CommandFactoryTests
{
    private Mock<IFileWriter> fileWriterMock;
    private Mock<IFileProvider> fileProviderMock;
    private Mock<IServiceProvider> serviceProviderMock;
    private Mock<IVersionService> versionServiceMock;
    private Mock<IConsoleService> consoleServiceMock;
    private CommandFactory commandFactory;
    private AppenderCommand? ac;
    private HelpCommand? hc;
    private InputFromFileCommand? iFF;
    private OutputToClipboardCommand? oTC;
    private ProcessCommand? pc;
    private ReplaceCommand? rc;
    private VersionCommand? vc;
    private OutputToFileCommand? oTF;
    [SetUp]
    public void Setup()
    {
        fileWriterMock= new Mock<IFileWriter>();
        fileProviderMock= new Mock<IFileProvider>();
        serviceProviderMock = new Mock<IServiceProvider>();
        versionServiceMock = new Mock<IVersionService>();
        consoleServiceMock= new Mock<IConsoleService>();

        commandFactory = new(new ICommand[]
        {
            ac = new AppenderCommand(serviceProviderMock.Object),
            hc = new HelpCommand(serviceProviderMock.Object, versionServiceMock.Object),
            iFF = new InputFromFileCommand(serviceProviderMock.Object, fileProviderMock.Object),
            oTC = new OutputToClipboardCommand(consoleServiceMock.Object,
                serviceProviderMock.Object),
            oTF = new OutputToFileCommand(consoleServiceMock.Object,
                serviceProviderMock.Object, fileWriterMock.Object),
            pc = new ProcessCommand(serviceProviderMock.Object),
            rc = new ReplaceCommand(serviceProviderMock.Object),
            vc = new VersionCommand(versionServiceMock.Object, serviceProviderMock.Object)
        });
    }

    [Test]
    public async Task Test1()
    {
        var commands = await commandFactory.GetCommands(new CommandLineArguments(null)
        {
            Input = "Meow",
            Output = "Woof"
        });

        Assert.That(commands, Contains.Item(iFF));
        Assert.That(commands, Contains.Item(pc));
        Assert.That(commands, Contains.Item(oTF));

        commands = await commandFactory.GetCommands(new CommandLineArguments(null)
        {
            Text = "Woof"
        });

        Assert.That(commands, Contains.Item(pc));
        Assert.That(commands, Contains.Item(oTC));

        commands = await commandFactory.GetCommands(new CommandLineArguments(null)
        {
            Text = "Woof",
            PrependValue = "Dog =",
            AppendValue = "!= Cat"
        });

        Assert.That(commands, Contains.Item(ac));
        Assert.That(commands, Contains.Item(pc));
        Assert.That(commands, Contains.Item(oTC));
    }
}