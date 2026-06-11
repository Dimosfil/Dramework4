param(
    [string]$PackageRoot = "Assets/IG",
    [string]$OutputDirectory = "Builds/UnityPackage"
)

$ErrorActionPreference = "Stop"

$repoRoot = (Resolve-Path -LiteralPath (Join-Path $PSScriptRoot "..\..")).Path
$pythonScript = Join-Path $PSScriptRoot "build_unitypackage_without_unity.py"

& (Join-Path $PSScriptRoot "validate-unitypackage-candidate.ps1") -PackageRoot $PackageRoot

python $pythonScript `
    --repo-root $repoRoot `
    --package-root $PackageRoot `
    --output-directory $OutputDirectory
