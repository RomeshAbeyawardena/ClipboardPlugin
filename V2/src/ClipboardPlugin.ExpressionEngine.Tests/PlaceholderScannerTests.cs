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

public class PlaceholderScannerTests
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
    public async Task Now_Expression()
    {
        var u = sut.Expression("now", culture);
        var t = await u.EvaluateAsync();
        Assert.That(t, Is.EqualTo(utcNow));

        //var u = sut.Expression("[now]", culture);
    }

    [Test]
    public void GetPlaceholderExpressions()
    {
        PlaceholderScanner scanner = new();
        var target = "{a} text {code} some other text {code 2}";
        var results = scanner.GetPlaceholderExpressions(target, '{', '}');
        Assert.That(results.Select(x => x.Item2), Contains.Item("a"));
        Assert.That(results.Select(x => x.Item2), Contains.Item("code"));
        Assert.That(results.Select(x => x.Item2), Contains.Item("code 2"));

        target = "{a text {code some other text} {code 2";
        
        Assert.Throws<IndexOutOfRangeException>(() =>
            results = scanner.GetPlaceholderExpressions(target, '{', '}'));

    }
}

