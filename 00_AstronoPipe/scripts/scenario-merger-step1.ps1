param()

$inputPath = "C:\Users\Marcu\source\repos\AstroWorkspace\AstronoSphere\AstronoData\01_CandidateData\Processed"
$outputPath = "C:\Users\Marcu\source\repos\AstroWorkspace\AstronoSphere\AstronoData\01_Seeds\Incoming"

function Convert-ScnToSeed {
    param(
        [string]$filePath
    )

    Write-Host "Processing $filePath..."

    $json = Get-Content $filePath -Raw | ConvertFrom-Json

    # --- Event (ORDER FIXED) ---
    $event = [ordered]@{
        Category    = $json.Event.Category
        Qualifier   = $json.Event.Comment
        Description = $json.Event.ApproximateJD
    }

    # --- Time (ORDER FIXED) ---
    $time = [ordered]@{
        StartJD   = $json.Core.Time.StartJD
        StopJD    = $json.Core.Time.StopJD
        Step      = $json.Core.Time.StepDays
        TimeScale = $json.Core.Time.TimeScale
    }

    # --- Observer (ORDER FIXED) ---
    $observer = [ordered]@{
        Type = $json.Core.Observer.Type
        Body = $json.Core.Observer.Body
    }

    # --- ObservedObject (ORDER FIXED) ---
    $observedObject = [ordered]@{
        BodyClass = "Planet"
        Targets   = $json.Core.Targets
    }

    # --- Frame (ORDER FIXED) ---
    $frame = [ordered]@{
        Type  = $json.Core.Frame.Type
        Epoch = $json.Core.Frame.Epoch
    }

    # --- Core (ORDER FIXED) ---
    $core = [ordered]@{
        Time           = $time
        Observer       = $observer
        ObservedObject = $observedObject
        Frame          = $frame
    }

    # --- Metadata (ORDER FIXED) ---
    $metadata = [ordered]@{
        Author   = "Scenario Merger"
        Priority = 1
        Status   = [ordered]@{
            Maturity   = "Released"
            Visibility = "Private"
        }
    }

    # --- SeedCandidate (ORDER FIXED) ---
    $seedCandidate = [ordered]@{
        Event    = $event
        Core     = $core
        Metadata = $metadata
        Notes    = "Scenario Merger: Generated automatically from 144 existing scenarios."
    }

    # --- SeedOrigin (ORDER FIXED) ---
    $seedOrigin = [ordered]@{
        ResultID     = "144-EXISTING-SCENARIOS_SCENARIO-TRANSFER"
        Reason       = "Merge of scenario candidates to seeds"
        Trigger      = "M1.9 Data Model Transfer"
        CreatedAtUtc = "2026-04-05T17:05:00Z"
    }

    # --- Final Wrapper (SPEC CORRECT) ---
    $root = [ordered]@{
        GeneratedSeeds = @(
            [ordered]@{
                SeedCandidate = $seedCandidate
                SeedOrigin    = $seedOrigin
            }
        )
    }

    # --- JSON erzeugen ---
    $jsonString = $root | ConvertTo-Json -Depth 10

    # --- Deterministische 2-Space-Formatierung ---
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
    $fileName = [System.IO.Path]::GetFileNameWithoutExtension($filePath)
    $outFile = Join-Path $outputPath ($fileName + ".json")

    [System.IO.File]::WriteAllLines($outFile, $formatted, [System.Text.Encoding]::UTF8)

    Write-Host "Created $outFile"
}

# --- Run ---
$files = Get-ChildItem $inputPath -Filter "SCN_*.json" | Sort-Object Name

foreach ($file in $files) {
    Convert-ScnToSeed -filePath $file.FullName
}