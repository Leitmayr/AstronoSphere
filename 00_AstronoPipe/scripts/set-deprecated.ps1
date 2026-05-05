# ============================================================
# FILE: 00_AstronoPipe/scripts/set-deprecated.ps1
# PURPOSE: Set Metadata.Status.Maturity = "Deprecated"
# ============================================================

param(
    [string]$Folder = ".\AstronoData\02_Experiments\Released",
    [int]$StartId = 73,
    [int]$EndId = 144,
    [switch]$DryRun
)

function Get-CatalogNumber {
    param($json)
    return $json.CatalogNumber
}

function Set-Deprecated {
    param($file)

    $raw = Get-Content $file.FullName -Raw
    $json = $raw | ConvertFrom-Json

    $catalog = Get-CatalogNumber $json

    if (-not $catalog) {
        Write-Host "Skipping (no CatalogNumber): $($file.Name)"
        return
    }

    $id = [int]($catalog -replace "AS-", "")

    if ($id -lt $StartId -or $id -gt $EndId) {
        return
    }

    Write-Host "Processing $catalog ($($file.Name))"

    if ($DryRun) {
        return
    }

    # Set Maturity
    $json.Metadata.Status.Maturity = "Deprecated"

    # Write back (preserve formatting as much as possible)
    $json | ConvertTo-Json -Depth 20 | Set-Content $file.FullName -Encoding UTF8
}

Write-Host "=== Set Deprecated ==="
Write-Host "Folder: $Folder"
Write-Host "Range: AS-$StartId to AS-$EndId"
Write-Host "DryRun: $DryRun"
Write-Host ""

$files = Get-ChildItem $Folder -Filter "*.json"

foreach ($file in $files) {
    Set-Deprecated $file
}

Write-Host ""
Write-Host "Done."