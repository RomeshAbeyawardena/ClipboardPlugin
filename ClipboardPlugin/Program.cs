using static System.Net.Mime.MediaTypeNames;

namespace ClipboardPlugin;

public partial class Program
{ 
    private static bool DisplayHelp(CommandLineArguments commandLineArguments)
    {
        if (commandLineArguments.Help.HasValue && commandLineArguments.Help.Value)
        {
            Console.WriteLine("Clipboard Plugin: Copies provided text to the clipboard:" +
                "\r\n\tUsage: ClipboardPlugin.exe [-s|--split] [splitCharacterOrString] [-t|--text] \"[Text to copy to clipboard]\"" +
                "\r\n Displays this help text:" +
                "\r\n\tHelp Usage: ClipboardPlugin.exe [-?|--help]");
            return true;
        }
        
        return false;
    }
    
    public async static Task CopyText(CommandLineArguments commandLineArguments)
    {
        WriteDebug("{0}", commandLineArguments);
        var currentClipboard = await TextCopy.ClipboardService.GetTextAsync();
        if (!string.IsNullOrWhiteSpace(commandLineArguments!.Text))
        {
            var textToCopy = commandLineArguments.Text;
            if (!string.IsNullOrEmpty(commandLineArguments.SplitString))
            {
                var splitString = commandLineArguments.Text.Split(commandLineArguments.SplitString);
                if (commandLineArguments.Index.HasValue)
                {
                    if (commandLineArguments.Index != -1)
                    {
                        textToCopy = splitString.ElementAtOrDefault(commandLineArguments.Index.Value);
                    }
                    else
                        textToCopy = splitString.LastOrDefault();
                }
                else
                    textToCopy = string.Join(",", splitString);
            }
            if(!string.IsNullOrWhiteSpace(textToCopy))
                await TextCopy.ClipboardService.SetTextAsync(textToCopy);
        }
        else
            WriteError("Aborted: No text to copy or content already copied to clipboard.");
        
    }
}