using ClipboardPlugin.Abstractions.Expressions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Globalization;

namespace ClipboardPlugin.ExpressionEngine.Tests;

[TestFixture]
internal class ConfigurationExpressionEngineTests
{
    private ConfigurationExpressionEngine engine;
    private DateTimeOffset utcNow;
    private Mock<ILogger<IExpressionEngine>> logger;
    private Mock<IApplicationSettings> applicationSettings;
    private CultureInfo culture;

    [SetUp]
    public void SetUp()
    {
        utcNow = new DateTimeOffset(2024, 09, 12, 12, 30, 23, TimeSpan.Zero);
        applicationSettings = new();

        applicationSettings.Setup(x => x.StartPlaceholder).Returns('{');
        applicationSettings.Setup(x => x.EndPlaceholder).Returns('}');
        logger = new();
        engine = new(new TestTimeProvider(utcNow), logger.Object, new PlaceholderScanner(), applicationSettings.Object);
        culture = CultureInfo.CreateSpecificCulture("en-gb");
    }

    [Test]
    public async Task Test1()
    {
        var result = await engine.Resolve("{now}", culture);
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo("12/09/2024 12:30:23 +00:00"));

        result = await engine.Resolve("{parseDate(now,'dd-MM-yyyy')}", culture);
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo("12-09-2024"));
    }
}
