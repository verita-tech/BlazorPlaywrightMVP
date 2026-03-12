using BlazorApp.UITests.Infrastructure;
using Microsoft.Playwright;
using Xunit;

namespace BlazorApp.UITests.Tests;

public class DashboardPageTests : PlaywrightTestBase
{
    [Fact]
    public async Task Dashboard_AsUserRole_CanAccessPage()
    {
        // Arrange
        var page = await CreateAuthenticatedPageAsync("UserTest", "User");

        // Act
        await page.GotoAsync($"{ServerAddress}/dashboard");
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        var title = await page.Locator("[data-testid='dashboard-title']").TextContentAsync();
        Assert.Equal("Dashboard", title);

        var message = await page.Locator("[data-testid='dashboard-message']").TextContentAsync();
        Assert.Contains("Nur Benutzer mit den Rollen", message);
    }

    [Fact]
    public async Task Dashboard_AsAdminRole_CanAccessPage()
    {
        // Arrange
        var page = await CreateAuthenticatedPageAsync("AdminTest", "Admin");

        // Act
        await page.GotoAsync($"{ServerAddress}/dashboard");
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        var title = await page.Locator("[data-testid='dashboard-title']").TextContentAsync();
        Assert.Equal("Dashboard", title);
    }

    [Fact]
    public async Task Dashboard_ShowsUserInformation()
    {
        // Arrange
        var page = await CreateAuthenticatedPageAsync("TestBenutzer", "User", "Admin");

        // Act
        await page.GotoAsync($"{ServerAddress}/dashboard");
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        var userName = await page.Locator("[data-testid='user-name']").TextContentAsync();
        Assert.Equal("TestBenutzer", userName);

        var rolesContainer = page.Locator("[data-testid='user-roles']");
        var rolesText = await rolesContainer.TextContentAsync();
        Assert.Contains("User", rolesText);
        Assert.Contains("Admin", rolesText);
    }

    [Fact]
    public async Task Dashboard_NavigationLink_VisibleForUser()
    {
        // Arrange
        var page = await CreateAuthenticatedPageAsync("UserTest", "User");

        // Act
        await page.GotoAsync(ServerAddress);
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        var dashboardLink = page.Locator("[data-testid='nav-dashboard']");
        await Assertions.Expect(dashboardLink).ToBeVisibleAsync();
    }

    [Fact]
    public async Task Dashboard_NavigationLink_VisibleForAdmin()
    {
        // Arrange
        var page = await CreateAuthenticatedPageAsync("AdminTest", "Admin");

        // Act
        await page.GotoAsync(ServerAddress);
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        var dashboardLink = page.Locator("[data-testid='nav-dashboard']");
        await Assertions.Expect(dashboardLink).ToBeVisibleAsync();
    }

    [Fact]
    public async Task Dashboard_CanNavigateFromHome()
    {
        // Arrange
        var page = await CreateAuthenticatedPageAsync("UserTest", "User");
        await page.GotoAsync(ServerAddress);
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Act
        await page.Locator("[data-testid='nav-dashboard']").ClickAsync();
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        var title = await page.Locator("[data-testid='dashboard-title']").TextContentAsync();
        Assert.Equal("Dashboard", title);
    }
}
