using Moq;

namespace ClipboardPlugin.ExpressionEngine.Tests;

public class TestTimeProvider(DateTimeOffset dateTimeOffset) : TimeProvider
{
    public override DateTimeOffset GetUtcNow()
    {
        return dateTimeOffset;
    }
}

public class Tests
{
    private ConfigurationExpressionEngine sut;
    private TestTimeProvider timeProvider;
    private DateTimeOffset utcNow;

    [SetUp]
    public void Setup()
    {
        utcNow = DateTimeOffset.Parse("12/09/2025 14:45:00", System.Globalization.CultureInfo.CreateSpecificCulture("en-gb"));
        timeProvider = new TestTimeProvider(utcNow);
        sut = new(timeProvider);
    }

    [Test]
    public async Task Test1()
    {
        var u = sut.Expression("[now]");
        var t = await u.EvaluateAsync();
        Assert.That(t, Is.EqualTo(utcNow));
    }
}
