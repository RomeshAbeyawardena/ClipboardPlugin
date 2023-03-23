namespace ClipboardPlugin;

public static class CommandOrder
{
    public const int INPUT_COMMAND = int.MinValue;
    public const int PROCESS_HIGH_PRIORITY_COMMAND = 25;
    public const int PROCESS_MEDIUM_PRIORITY_COMMAND = 50;
    public const int PROCESS_LOW_PRIORITY_COMMAND = 75;
    public const int PROCESS_LOWER_PRIORITY_COMMAND = 100;
    public const int OUTPUT_COMMAND = int.MaxValue;
}
