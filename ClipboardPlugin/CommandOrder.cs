namespace ClipboardPlugin;

public static class CommandOrder
{
   
    public const int INPUT_COMMAND = int.MinValue;
    public const int PROCESS_HIGH_PRIORITY_COMMAND = 25;
    public const int PROCESS_MEDIUM_PRIORITY_COMMAND = 50;
    public const int PROCESS_LOW_PRIORITY_COMMAND = 75;
    public const int PROCESS_LOWER_PRIORITY_COMMAND = 100;
    public const int OUTPUT_COMMAND = int.MaxValue;

    public static string Describe(int input)
    {
        return input switch
        {
            INPUT_COMMAND => nameof(INPUT_COMMAND),
            PROCESS_HIGH_PRIORITY_COMMAND => nameof(PROCESS_HIGH_PRIORITY_COMMAND),
            PROCESS_MEDIUM_PRIORITY_COMMAND => nameof(PROCESS_MEDIUM_PRIORITY_COMMAND),
            PROCESS_LOW_PRIORITY_COMMAND => nameof(PROCESS_LOW_PRIORITY_COMMAND),
            PROCESS_LOWER_PRIORITY_COMMAND => nameof(PROCESS_LOWER_PRIORITY_COMMAND),
            OUTPUT_COMMAND => nameof(OUTPUT_COMMAND),
            _ => string.Empty,
        };
    }
}
