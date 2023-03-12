namespace ClipboardPlugin.Contracts;

public interface IVersionService
{
    Version? GetVersion();
    string ReplaceVersion(string value, string? searchValue = "{version}");
}
