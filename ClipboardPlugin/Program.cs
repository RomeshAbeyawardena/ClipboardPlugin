using static System.Net.Mime.MediaTypeNames;

namespace ClipboardPlugin;

public partial class Program
{ 
    private static bool DisplayHelp(CommandLineArguments commandLineArguments)
    {
        if (commandLineArguments.Help.HasValue && commandLineArguments.Help.Value)
        {
            Console.WriteLine(Properties.Resources.HelpText);
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
            if (!string.IsNullOrWhiteSpace(textToCopy) && !textToCopy.Equals(currentClipboard))
                await TextCopy.ClipboardService.SetTextAsync(textToCopy);
            else
                WriteError(Properties.Resources.ErrorMessage_ContentIsNullOrEmptyOrAlreadyCopied);
        }
        else
            WriteError(Properties.Resources.ErrorMessage_ContentIsNullOrEmpty);
        
    }
}