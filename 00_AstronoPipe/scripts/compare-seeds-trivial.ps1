param(
    [string]$FolderA = "C:\Users\Marcu\source\repos\AstroWorkspace\AstronoSphere\AstronoData\01_Seeds\Prepared",
    [string]$FolderB = "C:\Users\Marcu\source\repos\AstroWorkspace\AstronoSphere\AstronoData\01_Seeds\Prepared\ScenarioMerger"
)

Write-Host "Comparing JSON files (logical match by SCN ID)..." -ForegroundColor Cyan

function Normalize-Json {
    param([string]$json)
    $obj = $json | ConvertFrom-Json
    return ($obj | ConvertTo-Json -Depth 100 -Compress)
}

# Map FolderB by SCN key (SCN_000123)
$mapB = @{}
Get-ChildItem $FolderB -Filter *.json | ForEach-Object {
    if ($_.Name -match "(SCN_\d{6})") {
        $mapB[$matches[1]] = $_.FullName
    }
}

$diffCount = 0
$okCount = 0

Get-ChildItem $FolderA -Filter *.json | ForEach-Object {

    if ($_.Name -notmatch "(SCN_\d{6})") {
        Write-Host "SKIP (no SCN id): $($_.Name)" -ForegroundColor Yellow
        return
    }

    $key = $matches[1]

    if (-not $mapB.ContainsKey($key)) {
        Write-Host "MISSING in B: $key" -ForegroundColor Yellow
        $diffCount++
        return
    }

    $fileA = $_.FullName
    $fileB = $mapB[$key]

    $jsonA = Get-Content $fileA -Raw
    $jsonB = Get-Content $fileB -Raw

    $normA = Normalize-Json $jsonA
    $normB = Normalize-Json $jsonB

    if ($normA -eq $normB) {
        $okCount++
    }
    else {
        Write-Host "DIFF: $key" -ForegroundColor Red
        $diffCount++
    }
}

Write-Host ""
Write-Host "RESULT:" -ForegroundColor Cyan
Write-Host "OK   : $okCount"
Write-Host "DIFF : $diffCount"

if ($diffCount -eq 0) {
    Write-Host "ALL FILES TRIVIALLY IDENTICAL OK" -ForegroundColor Green
}
else {
    Write-Host "DIFFERENCES FOUND" -ForegroundColor Red
}