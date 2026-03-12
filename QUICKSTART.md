# 🚀 Quick Start Guide

## Schritt 1: Playwright installieren

Nachdem Sie das Projekt gebaut haben, müssen Sie die Playwright-Browser installieren:

### Windows (PowerShell)
```powershell
cd tests\BlazorApp.UITests
dotnet build
pwsh bin\Debug\net8.0\playwright.ps1 install chromium
```

### Linux/macOS
```bash
cd tests/BlazorApp.UITests
dotnet build
playwright install chromium
```

## Schritt 2: Tests ausführen

```bash
# Alle Tests ausführen
dotnet test

# Nur UI-Tests
dotnet test tests/BlazorApp.UITests/BlazorApp.UITests.csproj

# Mit detaillierter Ausgabe
dotnet test --logger "console;verbosity=detailed"
```

## Schritt 3: Anwendung lokal testen

### Mit Test-Authentifizierung (empfohlen für lokales Testen)
```bash
cd src/BlazorApp
dotnet run --environment Testing
```

Dann öffnen Sie: https://localhost:5001

### Mit Windows-Authentifizierung (Produktionsmodus)
```bash
cd src/BlazorApp
dotnet run
```

**Hinweis**: Für Windows-Authentifizierung muss IIS Express oder ein Windows-Server verwendet werden.

## Schritt 4: Tests in der IDE ausführen

### Visual Studio
1. Öffnen Sie `BlazorPlaywrightMVP.sln`
2. Öffnen Sie den Test Explorer (Test > Test Explorer)
3. Klicken Sie auf "Run All Tests"

### Visual Studio Code
1. Installieren Sie die "C# Dev Kit" Extension
2. Öffnen Sie den Ordner `BlazorPlaywrightMVP`
3. Verwenden Sie die Test Explorer Ansicht

### Rider
1. Öffnen Sie `BlazorPlaywrightMVP.sln`
2. Rechtsklick auf das Test-Projekt > "Run All Tests"

## Schritt 5: Azure DevOps Pipeline einrichten

1. Erstellen Sie ein neues Repository in Azure DevOps
2. Pushen Sie den Code
3. Gehen Sie zu Pipelines > Create Pipeline
4. Wählen Sie "Azure Repos Git"
5. Wählen Sie Ihr Repository
6. Wählen Sie "Existing Azure Pipelines YAML file"
7. Wählen Sie `/azure-pipelines.yml`
8. Klicken Sie auf "Run"

## 🔍 Wichtige Dateien

- `src/BlazorApp/Program.cs` - Authentifizierungs-Konfiguration
- `tests/BlazorApp.UITests/Infrastructure/PlaywrightTestBase.cs` - Basis für alle Tests
- `tests/BlazorApp.UITests/Tests/` - Alle Testklassen
- `azure-pipelines.yml` - CI/CD Konfiguration

## 🎯 Erste Tests schreiben

Erstellen Sie eine neue Test-Datei in `tests/BlazorApp.UITests/Tests/`:

```csharp
using BlazorApp.UITests.Infrastructure;

namespace BlazorApp.UITests.Tests;

public class MeineTests : PlaywrightTestBase
{
    [Fact]
    public async Task MeinErsterTest()
    {
        // Arrange: Erstelle einen User mit Admin-Rolle
        var page = await CreateAuthenticatedPageAsync("TestUser", "Admin");

        // Act: Navigiere zur Admin-Seite
        await page.GotoAsync($"{ServerAddress}/admin");

        // Assert: Prüfe, ob der Titel korrekt ist
        var title = await page.Locator("[data-testid='admin-title']").TextContentAsync();
        Assert.Equal("Admin-Bereich", title);
    }
}
```

## 📋 Verfügbare Rollen

- `"User"` - Zugriff auf Dashboard
- `"Admin"` - Zugriff auf Admin-Bereich und Dashboard
- Keine Rolle - Nur Zugriff auf Home

## 🛠️ Troubleshooting

### Problem: "Playwright executable doesn't exist"
**Lösung**: Playwright-Browser installieren (siehe Schritt 1)

### Problem: Tests schlagen mit "Connection refused" fehl
**Lösung**: Stellen Sie sicher, dass die Anwendung im Testing-Modus läuft

### Problem: "System.InvalidOperationException: No service for type 'Microsoft.AspNetCore.Hosting.IWebHostEnvironment'"
**Lösung**: Stellen Sie sicher, dass `partial class Program` in Program.cs vorhanden ist

## 📚 Weiterführende Dokumentation

Siehe [README.md](README.md) für detaillierte Informationen zu:
- Authentifizierung & Autorisierung
- Best Practices
- Erweiterte Konfiguration
- Pipeline-Details
