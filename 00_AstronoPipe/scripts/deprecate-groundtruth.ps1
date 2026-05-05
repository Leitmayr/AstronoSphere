param(
    [switch]$DryRun
)

$source = ".\AstronoData\03_GroundTruth\Ephemeris\Horizons\Baseline"
$target = ".\AstronoData\03_GroundTruth\Ephemeris\Horizons\Deprecated"

$catalogNumbers = @(
    "AS-000059",
    "AS-000060",
    "AS-000061",
    "AS-000062",
    "AS-000063",
    "AS-000064",
    "AS-000065",
    "AS-000066",
    "AS-000067",
    "AS-000068",
    "AS-000069",
    "AS-000070",
    "AS-000071",
    "AS-000072"
)

Write-Host "=== Deprecate GroundTruth ==="
Write-Host "DryRun: $DryRun"

if (-not (Test-Path $target)) {
    if ($DryRun) {
        Write-Host "[DRY] Would create: $target"
    }
    else {
        New-Item -ItemType Directory -Path $target | Out-Null
    }
}

foreach ($catalog in $catalogNumbers) {
    Write-Host "Searching: $catalog"

    $match = Get-ChildItem $source -Filter *.json |
        Where-Object {
            $json = Get-Content $_.FullName -Raw | ConvertFrom-Json
            $json.ExperimentRef.CatalogNumber -eq $catalog
        } |
        Select-Object -First 1

    if ($null -eq $match) {
        Write-Warning "Not found: $catalog"
        continue
    }

    $dest = Join-Path $target $match.Name

    if ($DryRun) {
        Write-Host "[DRY] Move: $catalog -> $($match.Name)"
    }
    else {
        Move-Item $match.FullName $dest -Force
        Write-Host "Moved: $catalog -> $($match.Name)"
    }
}

Write-Host "Done."