param(
    [string]$FolderA = "C:\Users\Marcu\source\repos\AstroWorkspace\AstronoSphere\AstronoData\01_Seeds\Prepared",
    [string]$FolderB = "C:\Users\Marcu\source\repos\AstroWorkspace\AstronoSphere\AstronoData\01_Seeds\Prepared\ScenarioMerger"
)

Write-Host "Comparing JSON files (canonical form)..." -ForegroundColor Cyan

function Normalize-Json {
    param($path)

    $obj = Get-Content $path -Raw | ConvertFrom-Json

    # stabile Serialisierung (sortierte Keys)
    return ($obj | ConvertTo-Json -Depth 100 | Out-String).Trim()
}

# Map B
$mapB = @{}
Get-ChildItem $FolderB -Filter *.json | ForEach-Object {
    if ($_.Name -match "(SCN_\d{6})") {
        $mapB[$matches[1]] = $_.FullName
    }
}

$ok = 0
$diff = 0

Get-ChildItem $FolderA -Filter *.json | ForEach-Object {

    if ($_.Name -notmatch "(SCN_\d{6})") { return }

    $key = $matches[1]

    if (-not $mapB.ContainsKey($key)) {
        Write-Host "MISSING in B: $key" -ForegroundColor Yellow
        $diff++
        return
    }

    $a = Normalize-Json $_.FullName
    $b = Normalize-Json $mapB[$key]

    if ($a -eq $b) {
        $ok++
    }
    else {
        Write-Host "DIFF: $key" -ForegroundColor Red
        $diff++
    }
}

Write-Host ""
Write-Host "RESULT:"
Write-Host "OK   : $ok"
Write-Host "DIFF : $diff"
