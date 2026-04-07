$seedPath = "C:\Users\Marcu\source\repos\AstroWorkspace\AstronoSphere\AstronoData\01_Seeds\Incoming"
$asPath   = "C:\Users\Marcu\source\repos\AstroWorkspace\AstronoSphere\AstronoData\02_ObservationCatalog\Released"
$outPath  = "C:\Users\Marcu\source\repos\AstroWorkspace\AstronoSphere\AstronoData\01_Seeds\Prepared\ScenarioMerger"

$files = Get-ChildItem $seedPath -Filter "SCN_*.json"

foreach ($file in $files) {

    $seed = Get-Content $file.FullName -Raw | ConvertFrom-Json
    $seedCandidate = $seed.GeneratedSeeds[0].SeedCandidate

    $id = ($file.BaseName -replace "SCN_", "")
    $asFile = Join-Path $asPath ("AS-" + $id + ".json")

    if (-not (Test-Path $asFile)) {
        Write-Host "[MISSING] AS file for $($file.Name)"
        continue
    }

    $as = Get-Content $asFile -Raw | ConvertFrom-Json

    $experiment = @{
        SchemaVersion = "1.0"
        ExperimentID = "TO_BE_FILLED"
        CatalogNumber = $as.CatalogNumber
        CoreHash = "TO_BE_REPLACED"

        Core = $seedCandidate.Core
        Event = $seedCandidate.Event
        Metadata = $seedCandidate.Metadata
        Notes = $seedCandidate.Notes

        ScenarioCitation = $as.ScenarioCitation
        DatasetHeader    = $as.DatasetHeader
    }

    $json = $experiment | ConvertTo-Json -Depth 10
    $json = $json -replace "    ", "  "

    $outName = $file.BaseName + "_SM.json"
    $outFile = Join-Path $outPath $outName

    Set-Content $outFile $json -Encoding UTF8

    Write-Host "[OK] Experiment created: $outName"
}