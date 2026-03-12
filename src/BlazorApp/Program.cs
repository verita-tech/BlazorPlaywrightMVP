using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Authorization;
using BlazorApp.Components;
using BlazorApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Konfiguriere Authentifizierung basierend auf Umgebung
var useTestAuth = builder.Configuration.GetValue<bool>("UseTestAuthentication");

if (useTestAuth)
{
    // Test-Authentifizierung für CI/CD
    builder.Services.AddAuthentication("TestScheme")
        .AddScheme<TestAuthenticationSchemeOptions, TestAuthenticationHandler>("TestScheme", options => { });
}
else
{
    // Windows-Authentifizierung für Produktion
    builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
        .AddNegotiate();
}

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddCascadingAuthenticationState();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

// Für Playwright Tests zugänglich machen
public partial class Program { }
