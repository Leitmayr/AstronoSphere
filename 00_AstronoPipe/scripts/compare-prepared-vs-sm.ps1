$preparedPath = "C:\Users\Marcu\source\repos\AstroWorkspace\AstronoSphere\AstronoData\01_Seeds\Prepared"
$smPath       = Join-Path $preparedPath "ScenarioMerger"

$files = Get-ChildItem $preparedPath -Filter "SCN_*.json" | Where-Object {
    $_.Name -notlike "*_SM.json"
}

foreach ($file in $files) {

    $baseName = [System.IO.Path]::GetFileNameWithoutExtension($file.Name)
    $smFile   = Join-Path $smPath ($baseName + "_SM.json")

    if (-not (Test-Path $smFile)) {
        Write-Host "[MISSING] $($file.Name) -> no SM reference"
        continue
    }

    $hashA = Get-FileHash $file.FullName -Algorithm SHA256
    $hashB = Get-FileHash $smFile -Algorithm SHA256

    if ($hashA.Hash -eq $hashB.Hash) {
        Write-Host "[OK] $baseName"
    } else {
        Write-Host "[FAIL] $baseName"
    }
}