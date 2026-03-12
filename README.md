# Blazor Server .NET 8 mit Playwright UI-Tests MVP

Dieses Projekt demonstriert ein Blazor Server .NET 8 MVP mit automatisierten UI-Tests mittels Playwright und xUnit, das in einer Azure DevOps Pipeline auf Ubuntu ausgeführt wird.

## 🎯 Features

- **Blazor Server .NET 8** Anwendung
- **Windows-Authentifizierung** für Produktion
- **Test-Authentifizierung** für CI/CD Pipeline
- **Rollenbasierte Autorisierung** (User, Admin)
- **Playwright UI-Tests** mit xUnit
- **Azure DevOps Pipeline** für automatisierte Tests auf Ubuntu

## 📁 Projektstruktur

```
BlazorPlaywrightMVP/
├── src/
│   └── BlazorApp/                    # Hauptanwendung
│       ├── Components/               # Blazor-Komponenten
│       │   ├── Layout/              # Layout-Komponenten
│       │   ├── Pages/               # Seiten
│       │   └── _Imports.razor       # Globale Imports
│       ├── Services/                # Services
│       │   └── TestAuthenticationHandler.cs  # Test-Auth für CI/CD
│       ├── Program.cs               # App-Einstiegspunkt
│       └── appsettings*.json        # Konfigurationen
├── tests/
│   └── BlazorApp.UITests/           # UI-Tests
│       ├── Infrastructure/          # Test-Infrastruktur
│       │   ├── BlazorAppFactory.cs  # WebApplicationFactory
│       │   └── PlaywrightTestBase.cs # Basis-Test-Klasse
│       └── Tests/                   # Testklassen
└── azure-pipelines.yml              # CI/CD Pipeline
```

## 🔐 Authentifizierung & Autorisierung

### Produktions-Modus (Windows-Authentifizierung)

In der Produktion verwendet die Anwendung Windows-Authentifizierung über das `Negotiate` Schema:

```json
// appsettings.json
{
  "UseTestAuthentication": false
}
```

### Test-Modus (Header-basierte Authentifizierung)

Für CI/CD Tests wird eine Header-basierte Authentifizierung verwendet:

```json
// appsettings.Testing.json
{
  "UseTestAuthentication": true
}
```

Der `TestAuthenticationHandler` liest Benutzerinformationen aus HTTP-Headern:
- `X-Test-User`: Benutzername
- `X-Test-Roles`: Komma-getrennte Liste von Rollen

### Rollen

Das MVP implementiert zwei Rollen:
- **User**: Zugriff auf Home und Dashboard
- **Admin**: Zugriff auf Home, Dashboard und Admin-Bereich

## 🧪 UI-Tests schreiben

### Basis-Test-Klasse

Alle UI-Tests erben von `PlaywrightTestBase`, die folgende Funktionalität bereitstellt:

```csharp
public class MyTests : PlaywrightTestBase
{
    [Fact]
    public async Task MyTest()
    {
        // Erstelle eine authentifizierte Seite mit Rollen
        var page = await CreateAuthenticatedPageAsync("TestUser", "Admin");
        
        await page.GotoAsync($"{ServerAddress}/admin");
        
        // Assertions...
    }
}
```

### Rollenbasierte Tests

```csharp
// Benutzer mit "User"-Rolle
var page = await CreateAuthenticatedPageAsync("MaxMustermann", "User");

// Benutzer mit "Admin"-Rolle
var page = await CreateAuthenticatedPageAsync("AdminUser", "Admin");

// Benutzer mit mehreren Rollen
var page = await CreateAuthenticatedPageAsync("MultiUser", "User", "Admin");

// Benutzer ohne Rollen
var page = await CreateAuthenticatedPageAsync("GuestUser");
```

### Test-Beispiel

```csharp
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
}
```

## 🚀 Lokale Entwicklung

### Voraussetzungen

- .NET 8 SDK
- PowerShell (für Playwright-Installation)

### Setup

1. **Projekt klonen/erstellen**

2. **Playwright-Browser installieren**

```bash
cd tests/BlazorApp.UITests
pwsh bin/Debug/net8.0/playwright.ps1 install chromium
```

Oder unter Linux/macOS:
```bash
playwright install chromium
```

3. **Tests ausführen**

```bash
dotnet test tests/BlazorApp.UITests/BlazorApp.UITests.csproj
```

### Anwendung lokal starten

```bash
cd src/BlazorApp
dotnet run
```

Die Anwendung läuft standardmäßig mit Windows-Authentifizierung. Für lokale Tests ohne Windows-Auth:

```bash
dotnet run --environment Testing
```

## 🔄 CI/CD Pipeline

Die Azure DevOps Pipeline (`azure-pipelines.yml`) führt folgende Schritte aus:

### Build Stage
1. .NET SDK installieren
2. Dependencies wiederherstellen
3. Solution bauen
4. Anwendung publizieren
5. Artefakte veröffentlichen

### UI Tests Stage
1. .NET SDK installieren
2. Test-Dependencies wiederherstellen
3. Playwright-Browser installieren (Chromium)
4. Tests bauen
5. UI-Tests ausführen
6. Testergebnisse publizieren
7. Code-Coverage publizieren

### Pipeline einrichten

1. In Azure DevOps neues Pipeline erstellen
2. YAML-Datei `azure-pipelines.yml` verwenden
3. Pipeline speichern und ausführen

## 📊 Test-Organisation

### Test-Kategorien

- **HomePageTests**: Tests für die Startseite
- **DashboardPageTests**: Tests für das Dashboard (User + Admin)
- **AdminPageTests**: Tests für den Admin-Bereich (nur Admin)
- **AuthorizationTests**: Tests für Autorisierungsregeln

### Test-IDs (data-testid)

Die Komponenten verwenden `data-testid` Attribute für stabile Test-Selektoren:

```razor
<h1 data-testid="home-title">Willkommen</h1>
<span data-testid="username">@context.User.Identity?.Name</span>
<button data-testid="admin-action-btn">Admin-Aktion</button>
```

## 🎨 Best Practices

### Microsoft-Dokumentation befolgt

- ✅ [Blazor Authentication & Authorization](https://learn.microsoft.com/en-us/aspnet/core/blazor/security/)
- ✅ [ASP.NET Core Testing](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests)
- ✅ [WebApplicationFactory](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests#basic-tests-with-the-default-webapplicationfactory)

### Playwright Best Practices befolgt

- ✅ [Playwright for .NET](https://playwright.dev/dotnet/docs/intro)
- ✅ Stable Selektoren mit `data-testid`
- ✅ `WaitForLoadStateAsync` für Stabilität
- ✅ Headless-Modus in CI/CD
- ✅ `--no-sandbox` für Linux-Container

### xUnit Best Practices

- ✅ `IAsyncLifetime` für Setup/Cleanup
- ✅ Klare Arrange-Act-Assert Struktur
- ✅ Sprechende Test-Namen
- ✅ Isolation zwischen Tests

## 🔧 Erweiterte Konfiguration

### Playwright-Optionen anpassen

```csharp
Browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
{
    Headless = true,
    SlowMo = 100,  // Für Debugging
    Args = new[] { "--no-sandbox" }
});
```

### Weitere Rollen hinzufügen

1. In `Program.cs` Autorisierungsrichtlinien erweitern
2. Komponenten mit `[Authorize(Roles = "NewRole")]` schützen
3. Tests mit neuer Rolle erstellen:

```csharp
var page = await CreateAuthenticatedPageAsync("User", "NewRole");
```

## 📝 Hinweise

- Die Anwendung muss in der `Testing`-Umgebung laufen, damit die Header-basierte Authentifizierung funktioniert
- Playwright-Browser müssen vor den Tests installiert werden
- Die Pipeline verwendet Ubuntu, daher sind Linux-spezifische Playwright-Argumente notwendig
- `partial class Program` in `Program.cs` ist wichtig für `WebApplicationFactory`

## 🐛 Troubleshooting

### Playwright-Browser nicht gefunden

```bash
pwsh bin/Debug/net8.0/playwright.ps1 install --with-deps chromium
```

### Tests schlagen fehl mit Authentifizierungsfehler

Prüfen Sie, ob `appsettings.Testing.json` korrekt konfiguriert ist:
```json
{ "UseTestAuthentication": true }
```

### Pipeline-Fehler beim Browser-Installation

Stellen Sie sicher, dass die PowerShell-Task die Browser mit `--with-deps` installiert.

## 📚 Weitere Ressourcen

- [Blazor Dokumentation](https://learn.microsoft.com/en-us/aspnet/core/blazor/)
- [Playwright .NET Dokumentation](https://playwright.dev/dotnet/)
- [xUnit Dokumentation](https://xunit.net/)
- [Azure DevOps Pipelines](https://learn.microsoft.com/en-us/azure/devops/pipelines/)

## 📄 Lizenz

MIT License - Frei verwendbar für Ihre Projekte.
