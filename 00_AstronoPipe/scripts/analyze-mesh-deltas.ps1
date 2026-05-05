param(
    [Parameter(Mandatory=$true)]
    [string]$InputFile
)

Write-Host "=== Analyze Mesh Deltas ==="
Write-Host "InputFile: $InputFile"

if (-not (Test-Path $InputFile)) {
    throw "Input file not found: $InputFile"
}

$lines = Get-Content $InputFile
$records = New-Object System.Collections.Generic.List[object]

foreach ($line in $lines) {

    # Extract CatalogNumber
    if ($line -notmatch "AS-(\d{6})") {
        continue
    }

    $catalog = "AS-" + $matches[1]

    # Extract DeltaMax without using the Delta character
    if ($line -notmatch "max=([0-9]+[\.,][0-9]+E[\-\+][0-9]+)") {
        continue
    }

    $deltaText = $matches[1].Replace(",", ".")

    # Extract tolerance
    if ($line -notmatch "tol=([0-9]+[\.,][0-9]+E[\-\+][0-9]+)") {
        continue
    }

    $tolText = $matches[1].Replace(",", ".")

    $delta = [double]::Parse(
        $deltaText,
        [System.Globalization.CultureInfo]::InvariantCulture)

    $tol = [double]::Parse(
        $tolText,
        [System.Globalization.CultureInfo]::InvariantCulture)

    if ($tol -eq 0) {
        continue
    }

    $ratio = $delta / $tol

    $records.Add([PSCustomObject]@{
        Catalog = $catalog
        Delta   = $delta
        Tol     = $tol
        Ratio   = $ratio
    })
}

Write-Host ""
Write-Host "Parsed records: $($records.Count)"

if ($records.Count -eq 0) {
    Write-Warning "No records parsed."
    exit
}

Write-Host ""
Write-Host "=== GLOBAL RATIO STATS ==="
$records | Measure-Object Ratio -Minimum -Maximum -Average

Write-Host ""
Write-Host "=== WORST CASES TOP 20 ==="
$records |
    Sort-Object Ratio -Descending |
    Select-Object -First 20 |
    Format-Table -AutoSize

Write-Host ""
Write-Host "=== BORDERLINE RATIO GT 0.8 ==="
$records |
    Where-Object { $_.Ratio -gt 0.8 } |
    Sort-Object Ratio -Descending |
    Select-Object -First 50 |
    Format-Table -AutoSize

Write-Host ""
Write-Host "=== PER CATALOG MAX RATIO TOP 20 ==="
$records |
    Group-Object Catalog |
    ForEach-Object {
        $_.Group | Sort-Object Ratio -Descending | Select-Object -First 1
    } |
    Sort-Object Ratio -Descending |
    Select-Object -First 20 |
    Format-Table -AutoSize