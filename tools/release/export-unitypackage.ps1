param(
    [string]$UnityPath = "",
    [string]$OutputDirectory = "Builds/UnityPackage"
)

$ErrorActionPreference = "Stop"

$repoRoot = (Resolve-Path -LiteralPath (Join-Path $PSScriptRoot "..\..")).Path
$packageJsonPath = Join-Path $repoRoot "Assets\IG\package.json"
$projectVersionPath = Join-Path $repoRoot "ProjectSettings\ProjectVersion.txt"

& (Join-Path $PSScriptRoot "validate-unitypackage-candidate.ps1")

$package = Get-Content -LiteralPath $packageJsonPath -Raw -Encoding UTF8 | ConvertFrom-Json
$editorVersionLine = Get-Content -LiteralPath $projectVersionPath -TotalCount 1
$editorVersion = $editorVersionLine -replace "^m_EditorVersion:\s*", ""

if ([string]::IsNullOrWhiteSpace($UnityPath)) {
    $candidates = @(
        $env:UNITY_EDITOR,
        $env:UNITY_EDITOR_PATH,
        "C:\Program Files\Unity\Hub\Editor\$editorVersion\Editor\Unity.exe"
    ) | Where-Object { -not [string]::IsNullOrWhiteSpace($_) }

    foreach ($candidate in $candidates) {
        if (Test-Path -LiteralPath $candidate -PathType Leaf) {
            $UnityPath = $candidate
            break
        }
    }
}

if ([string]::IsNullOrWhiteSpace($UnityPath) -or -not (Test-Path -LiteralPath $UnityPath -PathType Leaf)) {
    throw "Unity executable was not found. Pass -UnityPath or set UNITY_EDITOR / UNITY_EDITOR_PATH."
}

$resolvedOutputDirectory = Join-Path $repoRoot $OutputDirectory
$artifactName = "Dramework4-$($package.version)-unity$($package.unity).unitypackage"
$artifactPath = Join-Path $resolvedOutputDirectory $artifactName

$arguments = @(
    "-batchmode",
    "-quit",
    "-projectPath", $repoRoot,
    "-executeMethod", "HappyCoder.Dramework4.Editor.Tools.Release.Dramework4UnityPackageExporter.ExportFromCommandLine",
    "-dramework4ReleaseOutput", $resolvedOutputDirectory
)

& $UnityPath @arguments
if ($LASTEXITCODE -ne 0) {
    throw "Unity export failed with exit code $LASTEXITCODE."
}

& (Join-Path $PSScriptRoot "inspect-unitypackage.ps1") -ArtifactPath $artifactPath
