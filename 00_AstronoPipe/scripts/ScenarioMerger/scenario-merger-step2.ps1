param(
    [string[]]$inputFiles
)

$seedPath = "C:\Users\Marcu\source\repos\AstroWorkspace\AstronoSphere\AstronoData\01_Seeds\Incoming"
$asPath   = "C:\Users\Marcu\source\repos\AstroWorkspace\AstronoSphere\AstronoData\02_ObservationCatalog\Released"
$outputPath = "C:\Users\Marcu\source\repos\AstroWorkspace\AstronoSphere\AstronoData\01_Seeds\Prepared\ScenarioMerger"


function Convert-SeedToExperiment {
    param(
        [string]$filePath
    )

    Write-Host "Processing $filePath..."

    $json = Get-Content $filePath -Raw | ConvertFrom-Json
    $seed = $json.GeneratedSeeds[0]
    $sc = $seed.SeedCandidate

    # --- Core ---
    $core = [ordered]@{
        Time = [ordered]@{
            StartJD   = $sc.Core.Time.StartJD
            StopJD    = $sc.Core.Time.StopJD
            Step      = $sc.Core.Time.Step
            TimeScale = $sc.Core.Time.TimeScale
        }
        Observer = [ordered]@{
            Type = $sc.Core.Observer.Type
            Body = $sc.Core.Observer.Body
        }
        ObservedObject = [ordered]@{
            BodyClass = $sc.Core.ObservedObject.BodyClass
            Targets   = $sc.Core.ObservedObject.Targets
        }
        Frame = [ordered]@{
            Type  = $sc.Core.Frame.Type
            Epoch = $sc.Core.Frame.Epoch
        }
    }

    # --- ExperimentID (STRICT SPEC: INTEGER JD, TRUNCATION) ---
    $startInt = [math]::Floor($core.Time.StartJD)
    $stopInt  = [math]::Floor($core.Time.StopJD)
    $step     = $core.Time.Step

    $experimentId = ("HELIO-J2000-TDB-{0}-{1}-{2}" -f $startInt, $stopInt, $step).ToUpper()

    # --- CatalogNumber ---
    $fileName = [System.IO.Path]::GetFileNameWithoutExtension($filePath)
    $catalogNumber = ($fileName -replace 'SCN_', 'AS-') -replace '_seed$', ''

    # --- Event ---
    $event = [ordered]@{
        Category    = $sc.Event.Category
        Qualifier   = $sc.Event.Qualifier
        Description = $sc.Event.Description
    }

    # --- Metadata ---
    $metadata = [ordered]@{
        Author   = $sc.Metadata.Author
        Priority = $sc.Metadata.Priority
        Status   = $sc.Metadata.Status
    }

    # --- Final Experiment ---
    $experiment = [ordered]@{
        SchemaVersion = "1.0"
        ExperimentID  = $experimentId
        CatalogNumber = $catalogNumber
        CoreHash      = "TO_BE_REPLACED"

        Core          = $core
        Event         = $event
        Metadata      = $metadata
        Notes         = $sc.Notes

        ScenarioCitation = $null
        DatasetHeader    = $null
    }

    # --- JSON ---
    $jsonString = $experiment | ConvertTo-Json -Depth 10

    # --- 2-SPACE FORMAT ---
    $indentLevel = 0
    $formatted = @()

    foreach ($line in ($jsonString -split "`r?`n")) {

        $trimmed = $line.Trim()

        if ($trimmed -match '^[\}\]]') {
            $indentLevel--
        }

        $formatted += (' ' * ($indentLevel * 2)) + $trimmed

        if ($trimmed -match '[\{\[]$') {
            $indentLevel++
        }
    }

    # --- Output ---
	$outFile = Join-Path $outputPath ($fileName + "_SM.json")

    [System.IO.File]::WriteAllLines($outFile, $formatted, [System.Text.Encoding]::UTF8)

    Write-Host "Created $outFile"
}

foreach ($file in $inputFiles) {
    Convert-SeedToExperiment -filePath $file
}