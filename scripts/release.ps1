param(
    [string]$Configuration = "Release",
    [string]$Version = ""
)

$ErrorActionPreference = "Stop"

Write-Host "Building TVChannelsHacked ($Configuration)..." -ForegroundColor Cyan

if (-not (Get-Command msbuild -ErrorAction SilentlyContinue)) {
    throw "MSBuild was not found. Please run from a Developer PowerShell for Visual Studio."
}

msbuild "TVChannelsHacked.csproj" /t:Build /p:Configuration=$Configuration

$outputPath = Join-Path $PSScriptRoot "..\bin\$Configuration\net48"
if (-not (Test-Path $outputPath)) {
    throw "Build output not found at $outputPath"
}

$releaseDir = Join-Path $PSScriptRoot "..\release"
New-Item -ItemType Directory -Path $releaseDir -Force | Out-Null

$resolvedVersion = if ([string]::IsNullOrWhiteSpace($Version)) { Get-Date -Format "yyyyMMdd-HHmmss" } else { $Version }
$zipName = "TVChannelsHacked-$resolvedVersion.zip"
$zipPath = Join-Path $releaseDir $zipName

Compress-Archive -Path (Join-Path $outputPath "*") -DestinationPath $zipPath -Force

$hash = Get-FileHash -Path $zipPath -Algorithm SHA256
$hashPath = "$zipPath.sha256"
"$($hash.Hash)  $zipName" | Set-Content -Path $hashPath -NoNewline

Write-Host "Release package created: $zipPath" -ForegroundColor Green
Write-Host "SHA256 file created: $hashPath" -ForegroundColor Green
