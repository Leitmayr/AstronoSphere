param(
    [string]$FolderA = "C:\Users\Marcu\source\repos\AstroWorkspace\AstronoSphere\AstronoData\01_Seeds\Prepared",
    [string]$FolderB = "C:\Users\Marcu\source\repos\AstroWorkspace\AstronoSphere\AstronoData\01_Seeds\Prepared\ScenarioMerger",
    [int]$MaxDiffsPerFile = 5
)

Write-Host "Comparing JSON files with deep diff..." -ForegroundColor Cyan

function Compare-ObjectDeep {
    param($a, $b, $path = "")

    $diffs = @()

    if ($null -eq $a -and $null -eq $b) {
        return $diffs
    }

    if ($null -eq $a -or $null -eq $b) {
        $diffs += "$path : A='$a' | B='$b'"
        return $diffs
    }

    if ($a.GetType().Name -ne $b.GetType().Name) {
        $diffs += "$path : TYPE MISMATCH A=$($a.GetType().Name) B=$($b.GetType().Name)"
        return $diffs
    }

    if ($a -is [System.Collections.IDictionary]) {
        $keys = ($a.Keys + $b.Keys) | Sort-Object -Unique
        foreach ($k in $keys) {
            $newPath = "$path.$k"
            $diffs += Compare-ObjectDeep $a[$k] $b[$k] $newPath
        }
        return $diffs
    }

    if ($a -is [System.Collections.IEnumerable] -and -not ($a -is [string])) {
        for ($i = 0; $i -lt [Math]::Max($a.Count, $b.Count); $i++) {
            $newPath = "$path[$i]"
            $valA = if ($i -lt $a.Count) { $a[$i] } else { $null }
            $valB = if ($i -lt $b.Count) { $b[$i] } else { $null }

            $diffs += Compare-ObjectDeep $valA $valB $newPath
        }
        return $diffs
    }

    if ($a -ne $b) {
        $diffs += "$path : A='$a' | B='$b'"
    }

    return $diffs
}

function Load-Json {
    param($path)
    return (Get-Content $path -Raw | ConvertFrom-Json)
}

# Map FolderB by SCN
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

    $objA = Load-Json $fileA
    $objB = Load-Json $fileB

    $diffs = Compare-ObjectDeep $objA $objB ""

    if ($diffs.Count -eq 0) {
        $okCount++
    }
    else {
        Write-Host ""
        Write-Host "DIFF: $key" -ForegroundColor Red

        $diffs | Select-Object -First $MaxDiffsPerFile | ForEach-Object {
            Write-Host "  $_"
        }

        if ($diffs.Count -gt $MaxDiffsPerFile) {
            Write-Host "  ... ($($diffs.Count) diffs total)"
        }

        $diffCount++
    }
}

Write-Host ""
Write-Host "RESULT:" -ForegroundColor Cyan
Write-Host "OK   : $okCount"
Write-Host "DIFF : $diffCount"