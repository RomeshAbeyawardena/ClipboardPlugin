namespace ClipboardPlugin.ExpressionEngine.Tests;

public class TestTimeProvider(DateTimeOffset? dateTimeOffset = null) : TimeProvider
{
    private DateTimeOffset? _dateTimeOffset = dateTimeOffset;
    
    public DateTimeOffset Date { set => _dateTimeOffset = value; }

    public override DateTimeOffset GetUtcNow()
    {
        return _dateTimeOffset.GetValueOrDefault();
    }
}

