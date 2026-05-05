param(
    [string]$RepoRoot = "",
    [string]$OutputFolder = "tmp"
)

Write-Host "=== Find Mesh Validation Experiments (MVH1/2/3) ==="

# --------------------------------------------------
# Resolve Repo Root
# --------------------------------------------------
if (-not $RepoRoot -or $RepoRoot -eq "") {
    $current = Get-Location

    while ($null -ne $current) {
        if (Test-Path (Join-Path $current "AstronoData")) {
            $RepoRoot = $current
            break
        }
        $current = Split-Path $current -Parent
        if ($current -eq "") { break }
    }
}

if (-not $RepoRoot) {
    throw "Could not determine RepoRoot. Please pass -RepoRoot."
}

Write-Host "RepoRoot: $RepoRoot"

# --------------------------------------------------
# Paths
# --------------------------------------------------
$experimentsPath = Join-Path $RepoRoot "AstronoData\02_Experiments\Released"
$outputPath      = Join-Path $RepoRoot $OutputFolder

if (-not (Test-Path $experimentsPath)) {
    throw "Experiments path not found: $experimentsPath"
}

if (-not (Test-Path $outputPath)) {
    New-Item -ItemType Directory -Path $outputPath | Out-Null
}

# --------------------------------------------------
# Scan
# --------------------------------------------------
$files = Get-ChildItem -Path $experimentsPath -Filter *.json -File

$matches = @()

foreach ($file in $files) {
    try {
        $json = Get-Content $file.FullName -Raw | ConvertFrom-Json

        if ($null -ne $json.Event -and
            $json.Event.Description -in @("MVH1","MVH2","MVH3")) {

            $catalog = $json.CatalogNumber
            $desc    = $json.Event.Description

            $newName = "$catalog`__$desc`__" + $file.Name
            $dest    = Join-Path $outputPath $newName

            Copy-Item $file.FullName $dest -Force

            Write-Host "Copied: $catalog ($desc)"

            $matches += $catalog
        }
    }
    catch {
        Write-Warning "Skipping invalid JSON: $($file.Name)"
    }
}

# --------------------------------------------------
# Summary
# --------------------------------------------------
Write-Host ""
Write-Host "=== Summary ==="
Write-Host "Total matches: $($matches.Count)"

$sorted = $matches | Sort-Object

foreach ($c in $sorted) {
    Write-Host $c
}

Write-Host "Done. Files copied to: $outputPath"