param(
    [string]$Configuration = "Release"
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

$timestamp = Get-Date -Format "yyyyMMdd-HHmmss"
$zipName = "TVChannelsHacked-$timestamp.zip"
$zipPath = Join-Path $releaseDir $zipName

Compress-Archive -Path (Join-Path $outputPath "*") -DestinationPath $zipPath -Force

Write-Host "Release package created: $zipPath" -ForegroundColor Green
