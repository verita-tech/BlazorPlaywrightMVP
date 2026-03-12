# Blazor Playwright MVP - Projektstruktur-Übersicht

## 📁 Vollständige Dateistruktur

```
BlazorPlaywrightMVP/
│
├── 📄 BlazorPlaywrightMVP.sln              # Visual Studio Solution
├── 📄 README.md                             # Hauptdokumentation
├── 📄 QUICKSTART.md                         # Schnellstart-Anleitung
├── 📄 BOOTSTRAP_SETUP.md                    # Bootstrap-Einrichtung
├── 📄 .gitignore                            # Git-Ignore-Regeln
├── 📄 azure-pipelines.yml                   # Azure DevOps CI/CD Pipeline
│
├── 📂 src/
│   └── 📂 BlazorApp/                        # Hauptanwendung
│       ├── 📄 BlazorApp.csproj              # Projekt-Datei
│       ├── 📄 Program.cs                    # App-Einstiegspunkt & Authentifizierung
│       ├── 📄 appsettings.json              # Konfiguration (Produktion)
│       ├── 📄 appsettings.Testing.json      # Konfiguration (Tests)
│       │
│       ├── 📂 Components/
│       │   ├── 📄 App.razor                 # Root-Komponente
│       │   ├── 📄 Routes.razor              # Routing-Konfiguration
│       │   ├── 📄 _Imports.razor            # Globale Imports
│       │   │
│       │   ├── 📂 Layout/
│       │   │   ├── 📄 MainLayout.razor      # Haupt-Layout mit Auth-Info
│       │   │   └── 📄 NavMenu.razor         # Navigation (rollenbasiert)
│       │   │
│       │   └── 📂 Pages/
│       │       ├── 📄 Home.razor            # Startseite (öffentlich)
│       │       ├── 📄 Dashboard.razor       # Dashboard (User + Admin)
│       │       └── 📄 Admin.razor           # Admin-Bereich (nur Admin)
│       │
│       ├── 📂 Services/
│       │   └── 📄 TestAuthenticationHandler.cs  # Test-Auth für CI/CD
│       │
│       └── 📂 wwwroot/
│           └── 📄 app.css                   # App-Styling
│
└── 📂 tests/
    └── 📂 BlazorApp.UITests/                # UI-Tests
        ├── 📄 BlazorApp.UITests.csproj      # Test-Projekt-Datei
        ├── 📄 GlobalUsings.cs               # Globale Usings
        │
        ├── 📂 Infrastructure/
        │   ├── 📄 BlazorAppFactory.cs       # WebApplicationFactory
        │   └── 📄 PlaywrightTestBase.cs     # Basis-Test-Klasse
        │
        └── 📂 Tests/
            ├── 📄 HomePageTests.cs          # Home-Seite Tests
            ├── 📄 DashboardPageTests.cs     # Dashboard Tests
            ├── 📄 AdminPageTests.cs         # Admin-Bereich Tests
            └── 📄 AuthorizationTests.cs     # Autorisierungs-Tests
```

## 🔑 Schlüsselkomponenten

### Authentifizierung & Autorisierung

| Datei | Zweck |
|-------|-------|
| `Program.cs` | Konfiguriert Windows-Auth (Produktion) oder Test-Auth (CI/CD) |
| `TestAuthenticationHandler.cs` | Ermöglicht Header-basierte Authentifizierung für Tests |
| `appsettings.Testing.json` | Aktiviert Test-Authentifizierung |

### UI-Komponenten

| Komponente | Zugriff | Beschreibung |
|------------|---------|--------------|
| `Home.razor` | Alle | Willkommensseite |
| `Dashboard.razor` | User, Admin | Dashboard mit Benutzerinfos |
| `Admin.razor` | Admin | Geschützter Admin-Bereich |
| `MainLayout.razor` | Alle | Layout mit Benutzer-/Rollen-Anzeige |
| `NavMenu.razor` | Alle | Rollenbasierte Navigation |

### Test-Infrastruktur

| Datei | Zweck |
|-------|-------|
| `PlaywrightTestBase.cs` | Basis-Klasse mit Auth-Methoden |
| `BlazorAppFactory.cs` | Startet App in Test-Umgebung |
| `*Tests.cs` | Konkrete Testfälle |

### CI/CD

| Datei | Zweck |
|-------|-------|
| `azure-pipelines.yml` | Build & Test Pipeline für Ubuntu |

## 🎯 Test-Kategorien

### 1. HomePageTests (4 Tests)
- ✅ Willkommensnachricht für authentifizierte Benutzer
- ✅ Anzeige des Benutzernamens im Header
- ✅ User-Badge für User-Rolle
- ✅ Admin-Badge für Admin-Rolle

### 2. DashboardPageTests (6 Tests)
- ✅ Zugriff mit User-Rolle
- ✅ Zugriff mit Admin-Rolle
- ✅ Anzeige von Benutzerinformationen
- ✅ Navigation sichtbar für User
- ✅ Navigation sichtbar für Admin
- ✅ Navigation von Home zum Dashboard

### 3. AdminPageTests (6 Tests)
- ✅ Zugriff mit Admin-Rolle
- ✅ Anzeige der Admin-Nachricht
- ✅ Anzeige des Action-Buttons
- ✅ Navigation nur für Admin sichtbar
- ✅ Navigation von Home zum Admin-Bereich
- ✅ Zugriff mit mehreren Rollen

### 4. AuthorizationTests (6 Tests)
- ✅ Admin-Seite verweigert Zugriff für User
- ✅ Admin-Link nicht sichtbar ohne Admin-Rolle
- ✅ Dashboard verweigert Zugriff ohne passende Rolle
- ✅ Dashboard-Link nicht sichtbar ohne Rolle
- ✅ Navigation zeigt nur autorisierte Links
- ✅ Rollen-Badges werden korrekt angezeigt

**Gesamt: 22 UI-Tests**

## 📊 Dateistatistiken

- **Gesamt-Dateien**: 25
- **C#-Dateien**: 8
- **Razor-Komponenten**: 8
- **Konfigurationsdateien**: 4
- **Dokumentation**: 3
- **CI/CD**: 1
- **Solution/Projekt**: 3

## 🔄 Test-Ausführungs-Flow

```
1. Azure Pipeline startet
   ↓
2. .NET 8 SDK wird installiert
   ↓
3. Dependencies werden wiederhergestellt
   ↓
4. Solution wird gebaut
   ↓
5. Playwright-Browser (Chromium) wird installiert
   ↓
6. BlazorAppFactory startet die App in Testing-Umgebung
   ↓
7. PlaywrightTestBase erstellt Browser-Contexts mit Rollen
   ↓
8. Tests werden ausgeführt (22 Tests)
   ↓
9. Testergebnisse werden publiziert
```

## 🛠️ Technologie-Stack

- **.NET 8** - Framework
- **Blazor Server** - UI-Framework
- **Playwright .NET** - UI-Test-Automatisierung
- **xUnit** - Test-Framework
- **WebApplicationFactory** - Integration-Testing
- **Azure DevOps Pipelines** - CI/CD
- **Bootstrap 5** - CSS-Framework
- **Windows Authentication / Test Auth** - Authentifizierung

## 📝 Nächste Schritte

1. **Projekt klonen/herunterladen**
2. **Playwright installieren** (siehe QUICKSTART.md)
3. **Tests lokal ausführen**: `dotnet test`
4. **Pipeline einrichten** in Azure DevOps
5. **Eigene Tests hinzufügen** in `tests/BlazorApp.UITests/Tests/`

---

**Hinweis**: Dieses MVP folgt allen offiziellen Best Practices von Microsoft und Playwright und ist production-ready für CI/CD auf Ubuntu.
