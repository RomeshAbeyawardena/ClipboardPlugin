using Moq;
using System.Globalization;

namespace ClipboardPlugin.ExpressionEngine.Tests;

[TestFixture]
public class PlaceholderScannerTests
{
    private PlaceholderScanner scanner;

    [SetUp]
    public void Setup()
    {
        scanner = new();
    }

    [Test]
    public void GetPlaceholderExpressions()
    {
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
        var target = string.Empty;
        var results = scanner.GetPlaceholderExpressions(target, '{', '}');
        Assert.That(results, Is.Empty);
    }

    [Test]
    public void NoPlaceholders()
    {
        var target = "This is a test string with no placeholders.";
        var results = scanner.GetPlaceholderExpressions(target, '{', '}');
        Assert.That(results, Is.Empty);
    }

    [Test]
    public void NestedPlaceholders()
    {
        var target = "{a {nested} placeholder}";
        Assert.Throws<IndexOutOfRangeException>(() =>
            scanner.GetPlaceholderExpressions(target, '{', '}'));
    }

    [Test]
    public void UnmatchedStartCharacter()
    {
        var target = "{a placeholder with no end";
        Assert.Throws<IndexOutOfRangeException>(() =>
            scanner.GetPlaceholderExpressions(target, '{', '}'));
    }

    [Test]
    public void UnmatchedEndCharacter()
    {
        var target = "a placeholder with no start}";
        Assert.Throws<IndexOutOfRangeException>(() =>
            scanner.GetPlaceholderExpressions(target, '{', '}'));
    }

    [Test]
    public void SinglePlaceholder()
    {
        var target = "{single}";
        var results = scanner.GetPlaceholderExpressions(target, '{', '}');
        Assert.That(results.Count(), Is.EqualTo(1));
        Assert.That(results.First().Item2, Is.EqualTo("single"));
    }

    [Test]
    public void SpecialCharactersInPlaceholder()
    {
        var target = "{!@#$%^&*()}";
        var results = scanner.GetPlaceholderExpressions(target, '{', '}');
        Assert.That(results.Count(), Is.EqualTo(1));
        Assert.That(results.First().Item2, Is.EqualTo("!@#$%^&*()"));
    }

    [Test]
    public void LargeInputPerformance()
    {
        var target = string.Concat(Enumerable.Repeat("{placeholder}", 10000));
        var results = scanner.GetPlaceholderExpressions(target, '{', '}');
        Assert.That(results.Count(), Is.EqualTo(10000));
    }
}

