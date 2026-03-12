using Microsoft.Playwright;
using Xunit;

namespace BlazorApp.UITests.Infrastructure;

public abstract class PlaywrightTestBase : IAsyncLifetime
{
    protected BlazorAppFactory Factory { get; private set; } = null!;
    protected HttpClient HttpClient { get; private set; } = null!;
    protected string ServerAddress { get; private set; } = null!;
    protected IPlaywright Playwright { get; private set; } = null!;
    protected IBrowser Browser { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        // Starte die Anwendung
        Factory = new BlazorAppFactory();
        HttpClient = Factory.CreateClient();
        ServerAddress = HttpClient.BaseAddress!.ToString().TrimEnd('/');

        // Initialisiere Playwright
        Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        Browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true,
            Args = new[] { "--no-sandbox", "--disable-setuid-sandbox" } // Für CI/CD
        });
    }

    public async Task DisposeAsync()
    {
        await Browser.DisposeAsync();
        Playwright.Dispose();
        await Factory.DisposeAsync();
        HttpClient.Dispose();
    }

    /// <summary>
    /// Erstellt einen neuen Browser-Context mit der angegebenen Rolle
    /// </summary>
    protected async Task<IBrowserContext> CreateAuthenticatedContextAsync(
        string username = "TestUser", 
        params string[] roles)
    {
        var context = await Browser.NewContextAsync(new BrowserNewContextOptions
        {
            ExtraHTTPHeaders = new Dictionary<string, string>
            {
                ["X-Test-User"] = username,
                ["X-Test-Roles"] = string.Join(",", roles)
            }
        });

        return context;
    }

    /// <summary>
    /// Erstellt eine neue Seite mit der angegebenen Rolle
    /// </summary>
    protected async Task<IPage> CreateAuthenticatedPageAsync(
        string username = "TestUser", 
        params string[] roles)
    {
        var context = await CreateAuthenticatedContextAsync(username, roles);
        var page = await context.NewPageAsync();
        return page;
    }
}
