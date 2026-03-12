using BlazorApp.UITests.Infrastructure;
using Microsoft.Playwright;
using Xunit;

namespace BlazorApp.UITests.Tests;

public class AuthorizationTests : PlaywrightTestBase
{
    [Fact]
    public async Task AdminPage_AsUserRole_ShowsNotAuthorized()
    {
        // Arrange - Benutzer mit nur "User"-Rolle
        var page = await CreateAuthenticatedPageAsync("RegularUser", "User");

        // Act
        await page.GotoAsync($"{ServerAddress}/admin");
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        var content = await page.ContentAsync();
        Assert.Contains("Nicht autorisiert", content);
    }

    [Fact]
    public async Task AdminPage_WithoutAdminRole_NavigationLinkNotVisible()
    {
        // Arrange
        var page = await CreateAuthenticatedPageAsync("RegularUser", "User");

        // Act
        await page.GotoAsync(ServerAddress);
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        var adminLink = page.Locator("[data-testid='nav-admin']");
        await Assertions.Expect(adminLink).Not.ToBeVisibleAsync();
    }

    [Fact]
    public async Task Dashboard_WithoutUserOrAdminRole_ShowsNotAuthorized()
    {
        // Arrange - Benutzer ohne passende Rolle
        var page = await CreateAuthenticatedPageAsync("GuestUser", "Guest");

        // Act
        await page.GotoAsync($"{ServerAddress}/dashboard");
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        var content = await page.ContentAsync();
        Assert.Contains("Nicht autorisiert", content);
    }

    [Fact]
    public async Task Dashboard_WithoutRoles_NavigationLinkNotVisible()
    {
        // Arrange - Benutzer ohne Rollen
        var page = await CreateAuthenticatedPageAsync("NoRoleUser");

        // Act
        await page.GotoAsync(ServerAddress);
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        var dashboardLink = page.Locator("[data-testid='nav-dashboard']");
        await Assertions.Expect(dashboardLink).Not.ToBeVisibleAsync();
    }

    [Fact]
    public async Task Navigation_ShowsOnlyAuthorizedLinks()
    {
        // Arrange
        var page = await CreateAuthenticatedPageAsync("UserTest", "User");
        await page.GotoAsync(ServerAddress);
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        var homeLink = page.Locator("[data-testid='nav-home']");
        var dashboardLink = page.Locator("[data-testid='nav-dashboard']");
        var adminLink = page.Locator("[data-testid='nav-admin']");

        await Assertions.Expect(homeLink).ToBeVisibleAsync();
        await Assertions.Expect(dashboardLink).ToBeVisibleAsync();
        await Assertions.Expect(adminLink).Not.ToBeVisibleAsync();
    }

    [Fact]
    public async Task RoleBadges_ShowCorrectly()
    {
        // Arrange
        var page = await CreateAuthenticatedPageAsync("TestUser", "User");
        await page.GotoAsync(ServerAddress);
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        var userBadge = page.Locator("[data-testid='user-badge']");
        var adminBadge = page.Locator("[data-testid='admin-badge']");

        await Assertions.Expect(userBadge).ToBeVisibleAsync();
        await Assertions.Expect(adminBadge).Not.ToBeVisibleAsync();
    }
}
