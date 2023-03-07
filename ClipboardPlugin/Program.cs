// See https://aka.ms/new-console-template for more information
namespace ClipboardPlugin;

public partial class Program
{ 
    public async static Task CopyText(CommandLineArguments commandLineArguments)
    {
        WriteDebug("{0}", commandLineArguments);

        if (!string.IsNullOrWhiteSpace(commandLineArguments!.Text))
        {
            await TextCopy.ClipboardService.SetTextAsync(commandLineArguments.Text);
        }
        else
            WriteError("Aborted: No text to copy");
    }
}