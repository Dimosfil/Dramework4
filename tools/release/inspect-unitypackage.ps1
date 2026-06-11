param(
    [Parameter(Mandatory = $true)]
    [string]$ArtifactPath
)

$ErrorActionPreference = "Stop"

function Get-TarString {
    param(
        [byte[]]$Buffer,
        [int]$Offset,
        [int]$Length
    )

    $bytes = $Buffer[$Offset..($Offset + $Length - 1)]
    $text = [System.Text.Encoding]::ASCII.GetString($bytes)
    $zeroIndex = $text.IndexOf([char]0)
    if ($zeroIndex -ge 0) {
        $text = $text.Substring(0, $zeroIndex)
    }

    return $text.Trim()
}

function Read-ExactBytes {
    param(
        [System.IO.Stream]$Stream,
        [long]$Size,
        [string]$EntryName
    )

    $content = New-Object byte[] $Size
    $contentRead = 0
    while ($contentRead -lt $Size) {
        $chunk = $Stream.Read($content, $contentRead, $Size - $contentRead)
        if ($chunk -eq 0) {
            throw "Unexpected end of tar content for $EntryName."
        }
        $contentRead += $chunk
    }

    return $content
}

function Skip-ExactBytes {
    param(
        [System.IO.Stream]$Stream,
        [long]$Size,
        [string]$EntryName
    )

    if ($Size -le 0) {
        return
    }

    $skipBuffer = New-Object byte[] ([Math]::Min($Size, 8192))
    $skipped = 0L
    while ($skipped -lt $Size) {
        $remaining = $Size - $skipped
        $toRead = [int][Math]::Min($skipBuffer.Length, $remaining)
        $chunk = $Stream.Read($skipBuffer, 0, $toRead)
        if ($chunk -eq 0) {
            throw "Unexpected end of tar content for $EntryName."
        }
        $skipped += $chunk
    }
}

$resolvedArtifactPath = (Resolve-Path -LiteralPath $ArtifactPath).Path
$fileStream = [System.IO.File]::OpenRead($resolvedArtifactPath)

try {
    $gzipStream = [System.IO.Compression.GZipStream]::new(
        $fileStream,
        [System.IO.Compression.CompressionMode]::Decompress)

    try {
        $buffer = New-Object byte[] 512
        $entries = New-Object System.Collections.Generic.List[string]
        $assetPaths = New-Object System.Collections.Generic.List[string]

        while ($true) {
            $read = 0
            while ($read -lt 512) {
                $chunk = $gzipStream.Read($buffer, $read, 512 - $read)
                if ($chunk -eq 0) {
                    break
                }
                $read += $chunk
            }

            if ($read -eq 0) {
                break
            }

            if ($read -ne 512) {
                throw "Unexpected end of tar header."
            }

            $isEmptyBlock = $true
            foreach ($byte in $buffer) {
                if ($byte -ne 0) {
                    $isEmptyBlock = $false
                    break
                }
            }

            if ($isEmptyBlock) {
                break
            }

            $name = Get-TarString $buffer 0 100
            $prefix = Get-TarString $buffer 345 155
            if (-not [string]::IsNullOrWhiteSpace($prefix)) {
                $name = "$prefix/$name"
            }

            $sizeText = Get-TarString $buffer 124 12
            $sizeText = ($sizeText -replace "[^0-7]", "")
            $size = if ([string]::IsNullOrWhiteSpace($sizeText)) { 0 } else { [Convert]::ToInt64($sizeText, 8) }

            $entries.Add($name)

            if ($size -gt 0) {
                if ($name -like "*/pathname") {
                    $content = Read-ExactBytes $gzipStream $size $name
                    $assetPath = ([System.Text.Encoding]::UTF8.GetString($content)).Trim()
                    if (-not [string]::IsNullOrWhiteSpace($assetPath)) {
                        $assetPaths.Add($assetPath.Replace("\", "/"))
                    }
                }
                else {
                    Skip-ExactBytes $gzipStream $size $name
                }
            }

            $padding = (512 - ($size % 512)) % 512
            if ($padding -gt 0) {
                Skip-ExactBytes $gzipStream $padding $name
            }
        }
    }
    finally {
        $gzipStream.Dispose()
    }
}
finally {
    $fileStream.Dispose()
}

$requiredAssetPaths = @(
    "Assets/IG",
    "Assets/IG/package.json",
    "Assets/IG/HappyCoder/Plugins/Dramework4/Code/Runtime",
    "Assets/IG/HappyCoder/Plugins/Dramework4/Code/Editor",
    "Assets/IG/HappyCoder/Plugins/Dramework4/Code/Tests/EditMode"
)

$forbiddenPatterns = @(
    "^Library/",
    "^Temp/",
    "^Obj/",
    "^Build/",
    "^Builds/",
    "^Logs/",
    "^UserSettings/",
    "^tools/",
    "^ProjectSettings/",
    "^Packages/",
    "^agent-work/",
    "\.sqlite$",
    "\.sqlite3$",
    "\.db$",
    "\.log$",
    "\.unitypackage$"
)

foreach ($required in $requiredAssetPaths) {
    if (-not ($assetPaths | Where-Object { $_ -eq $required -or $_.StartsWith("$required/") })) {
        throw "Required asset path was not found in artifact: $required"
    }
}

foreach ($assetPath in $assetPaths) {
    foreach ($pattern in $forbiddenPatterns) {
        if ($assetPath -match $pattern) {
            throw "Forbidden asset path was found in artifact: $assetPath"
        }
    }
}

[PSCustomObject]@{
    ArtifactPath = $resolvedArtifactPath
    TarEntryCount = $entries.Count
    AssetPathCount = $assetPaths.Count
    RootIncluded = [bool]($assetPaths | Where-Object { $_ -eq "Assets/IG" })
    PackageJsonIncluded = [bool]($assetPaths | Where-Object { $_ -eq "Assets/IG/package.json" })
} | Format-List
