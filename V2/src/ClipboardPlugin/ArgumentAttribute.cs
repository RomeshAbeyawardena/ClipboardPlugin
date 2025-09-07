namespace ClipboardPlugin;

[AttributeUsage(AttributeTargets.Property)]
public class ArgumentAttribute(params string[] aliases) : Attribute
{
    public IEnumerable<string> Aliases => aliases;
}
