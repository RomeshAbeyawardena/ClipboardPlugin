using Moq;
using System.Globalization;

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
    private CultureInfo culture;
    private ConfigurationExpressionEngine sut;
    private TestTimeProvider timeProvider;
    private DateTimeOffset utcNow;

    [SetUp]
    public void Setup()
    {
        culture = CultureInfo.CreateSpecificCulture("en-gb");
        utcNow = DateTimeOffset.Parse("12/09/2025 14:45:00", culture);
        timeProvider = new TestTimeProvider(utcNow);
        sut = new(timeProvider);
    }

    [Test]
    public async Task Test1()
    {
        var u = sut.Expression("[now]", culture);
        var t = await u.EvaluateAsync();
        Assert.That(t, Is.EqualTo(utcNow));
    }
}
