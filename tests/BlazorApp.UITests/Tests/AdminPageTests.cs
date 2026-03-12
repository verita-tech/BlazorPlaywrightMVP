using BlazorApp.UITests.Infrastructure;
using Microsoft.Playwright;
using Xunit;

namespace BlazorApp.UITests.Tests;

public class AdminPageTests : PlaywrightTestBase
{
    [Fact]
    public async Task AdminPage_AsAdminRole_CanAccessPage()
    {
        // Arrange
        var page = await CreateAuthenticatedPageAsync("AdminUser", "Admin");

        // Act
        await page.GotoAsync($"{ServerAddress}/admin");
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        var title = await page.Locator("[data-testid='admin-title']").TextContentAsync();
        Assert.Equal("Admin-Bereich", title);

        var warning = page.Locator("[data-testid='admin-warning']");
        await Assertions.Expect(warning).ToBeVisibleAsync();
    }

    [Fact]
    public async Task AdminPage_ShowsAdminMessage()
    {
        // Arrange
        var page = await CreateAuthenticatedPageAsync("SuperAdmin", "Admin");

        // Act
        await page.GotoAsync($"{ServerAddress}/admin");
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        var message = await page.Locator("[data-testid='admin-message']").TextContentAsync();
        Assert.Contains("SuperAdmin", message);
    }

    [Fact]
    public async Task AdminPage_ShowsAdminActionButton()
    {
        // Arrange
        var page = await CreateAuthenticatedPageAsync("AdminTest", "Admin");

        // Act
        await page.GotoAsync($"{ServerAddress}/admin");
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        var actionButton = page.Locator("[data-testid='admin-action-btn']");
        await Assertions.Expect(actionButton).ToBeVisibleAsync();
        
        var buttonText = await actionButton.TextContentAsync();
        Assert.Equal("Admin-Aktion ausführen", buttonText);
    }

    [Fact]
    public async Task AdminPage_NavigationLink_OnlyVisibleForAdmin()
    {
        // Arrange
        var page = await CreateAuthenticatedPageAsync("AdminTest", "Admin");

        // Act
        await page.GotoAsync(ServerAddress);
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        var adminLink = page.Locator("[data-testid='nav-admin']");
        await Assertions.Expect(adminLink).ToBeVisibleAsync();
    }

    [Fact]
    public async Task AdminPage_CanNavigateFromHome()
    {
        // Arrange
        var page = await CreateAuthenticatedPageAsync("AdminTest", "Admin");
        await page.GotoAsync(ServerAddress);
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Act
        await page.Locator("[data-testid='nav-admin']").ClickAsync();
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        var title = await page.Locator("[data-testid='admin-title']").TextContentAsync();
        Assert.Equal("Admin-Bereich", title);
    }

    [Fact]
    public async Task AdminPage_UserWithMultipleRoles_CanAccess()
    {
        // Arrange - Benutzer mit mehreren Rollen
        var page = await CreateAuthenticatedPageAsync("MultiRoleUser", "User", "Admin");

        // Act
        await page.GotoAsync($"{ServerAddress}/admin");
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        var title = await page.Locator("[data-testid='admin-title']").TextContentAsync();
        Assert.Equal("Admin-Bereich", title);
    }
}
