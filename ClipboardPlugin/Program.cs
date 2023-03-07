namespace ClipboardPlugin;

public partial class Program
{ 
    public async static Task CopyText(CommandLineArguments commandLineArguments)
    {
        if (commandLineArguments.Help.HasValue && commandLineArguments.Help.Value)
        {
            WriteDebug("Help");
        }

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