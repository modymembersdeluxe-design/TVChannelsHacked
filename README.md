# TVChannelsHacked (WinForms)

A longer Windows Forms desktop prototype for browsing live stream/satellite TV incident reports, including:

- Multi-select **Sources Videos** list.
- Timeline notes for **Coming Next**.
- Seeded sample records such as:
  - Cartoon Network Arabic incident-style promo/ident reports.
  - Spacetoon Arabic music segment interruption reports.
  - Additional worldwide channel examples.
- Filters by search, country, and content type.

## Windows 8.1 support

The project targets **.NET Framework 4.8** (`net48`) with WinForms to stay compatible with Windows 8.1 systems that have the .NET Framework runtime installed.

## Build (Visual Studio)

1. Open `TVChannelsHacked.csproj` in Visual Studio 2019/2022 on Windows.
2. Build and run.

## Release

### Local release package (Windows)

Use Developer PowerShell for Visual Studio:

```powershell
pwsh ./scripts/release.ps1
```

This builds `Release` and creates a zip in `./release/`.

### GitHub Actions build + release

A CI workflow is included at `.github/workflows/build-and-release.yml`.

- On tag push (`v*`), it builds on `windows-latest`, zips `bin/Release/net48`, uploads artifact, and publishes a GitHub Release.
- You can also run it manually with **workflow_dispatch** to verify build artifacts without creating a GitHub release.

## Notes

This tool is for archive/report browsing and mock incident tracking UI behavior only.
