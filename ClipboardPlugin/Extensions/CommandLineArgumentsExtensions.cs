namespace ClipboardPlugin.Extensions;

internal static class CommandLineArgumentsExtensions
{
    internal static string? HandleTextProcessing(this CommandLineArguments arguments)
    {
        var textToCopy = arguments.Text;
        if (!string.IsNullOrEmpty(arguments.Text) && !string.IsNullOrEmpty(arguments.SplitString))
        {
            var splitString = arguments.Text.Split(arguments.SplitString);
            if (arguments.Index.HasValue)
            {
                if (arguments.Index != -1)
                {
                    textToCopy = splitString.ElementAtOrDefault(arguments.Index.Value);
                }
                else
                    textToCopy = splitString.LastOrDefault();
            }
            else
                textToCopy = string.Join(",", splitString);
        }

        return textToCopy;
    }
}
