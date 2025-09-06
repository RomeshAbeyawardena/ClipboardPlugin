namespace ClipboardPlugin.Actions.Text;

public enum TextAction
{
    None,
    Replace,
    ExtractFilename
}

/// <summary>
/// 
/// </summary>
public static class TextActions
{
    public static IEnumerable<TextAction> Resolve(ClipboardArguments arguments)
    {
        var textActions = new List<TextAction>();
        if (arguments.IsReplacement)
        {
            textActions.Add(TextAction.Replace);
        }

        if (arguments.ExtractFileName)
        {
            textActions.Add(TextAction.ExtractFilename);
        }

        return textActions;
    }
}