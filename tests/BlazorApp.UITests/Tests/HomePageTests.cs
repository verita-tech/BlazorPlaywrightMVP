using BlazorApp.UITests.Infrastructure;
using Microsoft.Playwright;
using Xunit;

namespace BlazorApp.UITests.Tests;

public class HomePageTests : PlaywrightTestBase
{
    [Fact]
    public async Task HomePage_AsAuthenticatedUser_DisplaysWelcomeMessage()
    {
        // Arrange
        var page = await CreateAuthenticatedPageAsync("MaxMustermann", "User");

        // Act
        await page.GotoAsync(ServerAddress);
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        var title = await page.Locator("[data-testid='home-title']").TextContentAsync();
        Assert.Equal("Willkommen", title);

        var message = await page.Locator("[data-testid='home-message']").TextContentAsync();
        Assert.Contains("MaxMustermann", message);
        Assert.Contains("Sie sind angemeldet", message);
    }

    [Fact]
    public async Task HomePage_ShowsUsernameInHeader()
    {
        // Arrange
        var page = await CreateAuthenticatedPageAsync("JohnDoe", "Admin");

        // Act
        await page.GotoAsync(ServerAddress);
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        var username = await page.Locator("[data-testid='username']").TextContentAsync();
        Assert.Equal("JohnDoe", username);
    }

    [Fact]
    public async Task HomePage_AsUser_ShowsUserBadge()
    {
        // Arrange
        var page = await CreateAuthenticatedPageAsync("UserTest", "User");

        // Act
        await page.GotoAsync(ServerAddress);
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        var userBadge = page.Locator("[data-testid='user-badge']");
        await Assertions.Expect(userBadge).ToBeVisibleAsync();
        
        var badgeText = await userBadge.TextContentAsync();
        Assert.Equal("User", badgeText);
    }

    [Fact]
    public async Task HomePage_AsAdmin_ShowsAdminBadge()
    {
        // Arrange
        var page = await CreateAuthenticatedPageAsync("AdminTest", "Admin");

        // Act
        await page.GotoAsync(ServerAddress);
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        var adminBadge = page.Locator("[data-testid='admin-badge']");
        await Assertions.Expect(adminBadge).ToBeVisibleAsync();
        
        var badgeText = await adminBadge.TextContentAsync();
        Assert.Equal("Admin", badgeText);
    }
}
