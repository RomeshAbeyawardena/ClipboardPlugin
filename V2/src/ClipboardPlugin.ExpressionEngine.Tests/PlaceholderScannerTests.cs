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

[TestFixture]
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

        target = "{a text {code some other text} code 2}";
        
        Assert.Throws<IndexOutOfRangeException>(() =>
            results = scanner.GetPlaceholderExpressions(target, '{', '}'));

        target = "{a} text {code some other text} code 2}";

        Assert.Throws<IndexOutOfRangeException>(() =>
            results = scanner.GetPlaceholderExpressions(target, '{', '}'));
    }

    [Test]
    public void EmptyInput()
    {
        PlaceholderScanner scanner = new();
        var target = string.Empty;
        var results = scanner.GetPlaceholderExpressions(target, '{', '}');
        Assert.That(results, Is.Empty);
    }

    [Test]
    public void NoPlaceholders()
    {
        PlaceholderScanner scanner = new();
        var target = "This is a test string with no placeholders.";
        var results = scanner.GetPlaceholderExpressions(target, '{', '}');
        Assert.That(results, Is.Empty);
    }

    [Test]
    public void NestedPlaceholders()
    {
        PlaceholderScanner scanner = new();
        var target = "{a {nested} placeholder}";
        Assert.Throws<IndexOutOfRangeException>(() =>
            scanner.GetPlaceholderExpressions(target, '{', '}'));
    }

    [Test]
    public void UnmatchedStartCharacter()
    {
        PlaceholderScanner scanner = new();
        var target = "{a placeholder with no end";
        Assert.Throws<IndexOutOfRangeException>(() =>
            scanner.GetPlaceholderExpressions(target, '{', '}'));
    }

    [Test]
    public void UnmatchedEndCharacter()
    {
        PlaceholderScanner scanner = new();
        var target = "a placeholder with no start}";
        Assert.Throws<IndexOutOfRangeException>(() =>
            scanner.GetPlaceholderExpressions(target, '{', '}'));
    }

    [Test]
    public void SinglePlaceholder()
    {
        PlaceholderScanner scanner = new();
        var target = "{single}";
        var results = scanner.GetPlaceholderExpressions(target, '{', '}');
        Assert.That(results.Count(), Is.EqualTo(1));
        Assert.That(results.First().Item2, Is.EqualTo("single"));
    }

    [Test]
    public void SpecialCharactersInPlaceholder()
    {
        PlaceholderScanner scanner = new();
        var target = "{!@#$%^&*()}";
        var results = scanner.GetPlaceholderExpressions(target, '{', '}');
        Assert.That(results.Count(), Is.EqualTo(1));
        Assert.That(results.First().Item2, Is.EqualTo("!@#$%^&*()"));
    }

    [Test]
    public void LargeInputPerformance()
    {
        PlaceholderScanner scanner = new();
        var target = string.Concat(Enumerable.Repeat("{placeholder}", 10000));
        var results = scanner.GetPlaceholderExpressions(target, '{', '}');
        Assert.That(results.Count(), Is.EqualTo(10000));
    }
}

