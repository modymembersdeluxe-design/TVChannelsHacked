# TVChannelsHacked (WinForms)

A longer Windows Forms desktop prototype for browsing live stream/satellite TV incident reports, including:

- Multi-select **Sources Videos** list.
- Timeline notes for **Coming Next**.
- New Spacetoon **Space Power block 10:29:23 PM** coming-next hacked scenario in seeded data.
- Seeded sample records such as:
  - Cartoon Network Arabic incident-style promo/ident reports.
  - Spacetoon Arabic music segment interruption reports.
  - Additional worldwide channel examples.
- Filters by search, country, and content type.
- "Use Video Played" action to mark selected hacked source videos as played.
- Added tabs for **Music Audio Hacked** sources and **Shows Hacked Cuts** lists.
- Source video formats for hacked-played scenarios: **mp4, avi, wmv, 3gp**.
- Source audio formats for hacked-played scenarios: **mp3, wav, ogg**.
- Expanded channel catalog includes Cartoon Network, Disney/Disney Jr./Disney XD, Nickelodeon family, CBeebies/CBBC, Boomerang, Pop networks, Discovery Family/Kids, Gulli/Okoo/Canal+ Kids/Canal J, Cartoonito, Paramount Kids, Spacetoon, MBC 3, and Nickelodeon Arabia.

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

This builds `Release`, creates a zip in `./release/`, and writes a matching `.sha256` checksum file.

### GitHub Actions build + release

A CI workflow is included at `.github/workflows/build-and-release.yml`.

- On tag push (`v*`), it builds on `windows-latest`, zips `bin/Release/net48`, uploads artifact, and publishes a GitHub Release.
- You can also run it manually with **workflow_dispatch** to verify build artifacts without creating a GitHub release.
To publish a release from CI:

```bash
git tag v1.0.0
git push origin v1.0.0
```

The workflow publishes both the release zip and its SHA256 file.


## Notes

This tool is for archive/report browsing and mock incident tracking UI behavior only.
