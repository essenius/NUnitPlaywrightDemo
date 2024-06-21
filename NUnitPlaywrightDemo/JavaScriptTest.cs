namespace NUnitPlaywrightDemo;


[Parallelizable(ParallelScope.Self), TestFixture]
public partial class JavaScriptTest : PageTest
{
    private static string TestPage => AppConfig.Get("TestPage");

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        Assert.That(TestPage, Is.Not.Null.Or.Empty, "TestPage was not set");
    }

    [SetUp]
    public async Task TestInitialize()
    {
        Page.Context.SetDefaultTimeout(10_000);
        await Page.GotoAsync(AppConfig.Get("TestPage"));
    }

    [Test]
    public async Task HasTitle()
    {
        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(TitleRegex());
    }

    [Test]
    public async Task FibonacciRuns()
    {
        // check if the page is loaded
        var href = await Page.EvaluateAsync<string>("document.location.href");
        Assert.That(href, Is.EqualTo(TestPage));

        // check if the Fibonacci function is available and that it returns the correct value
        var result = await Page.EvaluateAsync<int>("Fibonacci(10)");
        Assert.That(result, Is.EqualTo(55));
    }

    [GeneratedRegex("Selenium Fixture Test Page")]
    private static partial Regex TitleRegex();
}
