using ClipboardPlugin.Extensions;
using Microsoft.Extensions.FileProviders;
using RST.Attributes;

namespace ClipboardPlugin.Commands;

[Register]
public class InputFromFileCommand : CommandBase
{
    private readonly IFileProvider fileProvider;

    public InputFromFileCommand(IServiceProvider serviceProvider, IFileProvider fileProvider) 
        : base(serviceProvider, "input", string.Empty, CommandOrder.INPUT_COMMAND)
    {
        this.fileProvider = fileProvider;
    }

    public override async Task Execute(CommandLineArguments arguments, string? command = null)
    {
        if (string.IsNullOrWhiteSpace(arguments.Input))
        {
            await Task.CompletedTask;
        }

        var fileInfo = fileProvider.GetFileInfo(arguments.Input);
        
        if (fileInfo.Exists)
        {
            using var stream = fileInfo.CreateReadStream();
            using var streamReader = new StreamReader(stream);
            arguments.Text = await streamReader.ReadToEndAsync();
        }
    }

    protected override Task<bool> OnCanExecute(CommandLineArguments arguments, string? command = null)
    {
        return this.CalculateCanExecute(arguments, !string.IsNullOrWhiteSpace(arguments.Input),
            false);
    }
}
