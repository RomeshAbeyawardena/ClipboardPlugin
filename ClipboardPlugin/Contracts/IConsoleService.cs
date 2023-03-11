namespace ClipboardPlugin.Contracts;

internal interface IConsoleService
{
    void WriteError(string message, params object[] args);
    void WriteDebug(string message, params object[] args);
}

