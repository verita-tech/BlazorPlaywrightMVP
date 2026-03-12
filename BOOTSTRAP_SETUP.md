# Bootstrap Setup

Für dieses MVP wird Bootstrap 5 über CDN eingebunden.

## Option 1: CDN (Empfohlen für MVP)

Fügen Sie in `Components/App.razor` folgenden Link hinzu:

```html
<head>
    <!-- ... andere head-Elemente ... -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
```

## Option 2: Lokale Installation via npm

Falls Sie Bootstrap lokal installieren möchten:

```bash
cd src/BlazorApp/wwwroot
npm init -y
npm install bootstrap
```

Dann kopieren Sie die CSS-Datei:
```bash
cp node_modules/bootstrap/dist/css/bootstrap.min.css wwwroot/bootstrap/
```

## Alternative: Verwendung ohne Bootstrap

Die Anwendung funktioniert auch ohne Bootstrap, allerdings ohne das schöne Styling. Die Funktionalität und Tests bleiben davon unberührt.

Für das MVP empfehlen wir Option 1 (CDN), da sie am einfachsten einzurichten ist.
