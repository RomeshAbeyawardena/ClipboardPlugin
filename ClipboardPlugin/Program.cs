namespace ClipboardPlugin;

public partial class Program
{ 
    private static bool DisplayHelp(CommandLineArguments commandLineArguments)
    {
        if (commandLineArguments.Help.HasValue && commandLineArguments.Help.Value)
        {
            Console.WriteLine("Clipboard Plugin: Copies provided text to the clipboard" +
                "\r\n\tUsage: ClipboardPlugin.exe [-t|--text] \"[Text to copy to clipboard]\"" +
                "\r\n Displays this help text" +
                "\r\n\tHelp Usage: ClipboardPlugin.exe [-?|--help]");
            return true;
        }

        return false;
    }

    public async static Task CopyText(CommandLineArguments commandLineArguments)
    {
        WriteDebug("{0}", commandLineArguments);
        var currentClipboard = await TextCopy.ClipboardService.GetTextAsync();
        if (!string.IsNullOrWhiteSpace(commandLineArguments!.Text)
            && (string.IsNullOrWhiteSpace(currentClipboard) || !currentClipboard.Equals(commandLineArguments.Text)))
        {
            await TextCopy.ClipboardService.SetTextAsync(commandLineArguments.Text);
        }
        else
            WriteError("Aborted: No text to copy or content already copied to clipboard.");
        
    }
}