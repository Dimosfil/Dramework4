param(
    [string]$PackageRoot = "Assets/IG"
)

$ErrorActionPreference = "Stop"

$repoRoot = (Resolve-Path -LiteralPath (Join-Path $PSScriptRoot "..\..")).Path
$packagePath = (Resolve-Path -LiteralPath (Join-Path $repoRoot $PackageRoot)).Path
$packageJsonPath = Join-Path $packagePath "package.json"
$projectVersionPath = Join-Path $repoRoot "ProjectSettings\ProjectVersion.txt"

if (-not (Test-Path -LiteralPath $packageJsonPath -PathType Leaf)) {
    throw "Package metadata not found: $packageJsonPath"
}

if (-not (Test-Path -LiteralPath $projectVersionPath -PathType Leaf)) {
    throw "Unity project version file not found: $projectVersionPath"
}

$package = Get-Content -LiteralPath $packageJsonPath -Raw -Encoding UTF8 | ConvertFrom-Json
$projectVersion = Get-Content -LiteralPath $projectVersionPath -TotalCount 2
$editorVersionLine = $projectVersion | Where-Object { $_ -like "m_EditorVersion:*" } | Select-Object -First 1

if (-not $package.name -or -not $package.version -or -not $package.unity) {
    throw "package.json must define name, version, and unity."
}

if ($package.unity -notlike "2022.3*") {
    throw "Expected Unity baseline 2022.3, found '$($package.unity)'."
}

if (-not $editorVersionLine -or $editorVersionLine -notmatch "2022\.3") {
    throw "Expected current Unity editor version 2022.3+, found '$editorVersionLine'."
}

$requiredPaths = @(
    "package.json",
    "HappyCoder\Plugins\Dramework4\Code\Runtime",
    "HappyCoder\Plugins\Dramework4\Code\Editor",
    "HappyCoder\Plugins\Dramework4\Code\Tests\EditMode"
)

foreach ($relativePath in $requiredPaths) {
    $candidate = Join-Path $packagePath $relativePath
    if (-not (Test-Path -LiteralPath $candidate)) {
        throw "Required package path is missing: $relativePath"
    }
}

$forbiddenPathParts = @(
    "\Library\",
    "\Temp\",
    "\Obj\",
    "\Build\",
    "\Builds\",
    "\Logs\",
    "\UserSettings\",
    "\.git\",
    "\.vs\",
    "\.idea\",
    "\tools\",
    "\ProjectSettings\",
    "\Packages\",
    "\agent-work\"
)

$forbiddenExtensions = @(".sqlite", ".sqlite3", ".db", ".log", ".unitypackage")
$files = Get-ChildItem -LiteralPath $packagePath -Recurse -File -Force

foreach ($file in $files) {
    $normalized = $file.FullName.Replace("/", "\")

    foreach ($part in $forbiddenPathParts) {
        if ($normalized.Contains($part)) {
            throw "Forbidden path included in package candidate: $($file.FullName)"
        }
    }

    if ($forbiddenExtensions -contains $file.Extension.ToLowerInvariant()) {
        throw "Forbidden file type included in package candidate: $($file.FullName)"
    }
}

$artifactName = "Dramework4-$($package.version)-unity$($package.unity).unitypackage"

[PSCustomObject]@{
    PackageName = $package.name
    DisplayName = $package.displayName
    Version = $package.version
    UnityBaseline = $package.unity
    CurrentEditor = ($editorVersionLine -replace "^m_EditorVersion:\s*", "")
    PackageRoot = $PackageRoot
    CandidateFileCount = $files.Count
    ArtifactName = $artifactName
} | Format-List
