param(
    [string[]]$inputFiles
)

function Convert-ScnToSeed {
    param(
        [string]$filePath
    )

    Write-Host "Processing $filePath..."

    $json = Get-Content $filePath -Raw | ConvertFrom-Json

    # --- Event ---
    $event = @{
        Category    = $json.Event.Category
        Qualifier   = $null
        Description = $json.Event.Comment
    }

    # --- Core.Time ---
    $time = @{
        StartJD   = $json.Core.Time.StartJD
        StopJD    = $json.Core.Time.StopJD
        Step      = $json.Core.Time.StepDays
        TimeScale = $json.Core.Time.TimeScale
    }

    # --- Observer ---
    $observer = @{
        Type = $json.Core.Observer.Type
        Body = $json.Core.Observer.Body
    }

    # --- ObservedObject ---
    $observedObject = @{
        BodyClass = "Planet"
        Targets   = $json.Targets
    }

    # --- Frame ---
    $frame = @{
        Type  = $json.Core.Frame.Type
        Epoch = $json.Core.Frame.Epoch
    }

    # --- Core ---
    $core = @{
        Time           = $time
        Observer       = $observer
        ObservedObject = $observedObject
        Frame          = $frame
    }

    # --- Metadata (Footer) ---
    $metadata = @{
        Author   = "Sceneario Merger"
        Priority = 1
        Status   = @{
            Maturity   = "Released"
            Visibility = "Private"
        }
    }

    # --- SeedOrigin (Footer) ---
    $seedOrigin = @{
        ResultID     = "144-EXISTING-SCENARIOS_SCENARIO-TRANSFER"
        Reason       = "Merge of scenario candidates to sseds"
        Trigger      = "M1.9 Data Model transfer"
        CreatedAtUtc = "2026-04-05T16:00:00Z"
    }

    # --- Final Seed ---
    $seed = @{
        SeedCandidate = @{
            Event    = $event
            Core     = $core
            Metadata = $metadata
            Notes    = "Sceneario Merger: Generated automatically from 144 existing scenarios."
        }
        SeedOrigin = $seedOrigin
    }

    # --- Output filename ---
    $fileName = [System.IO.Path]::GetFileNameWithoutExtension($filePath)
    $outFile = "$fileName`_seed.json"

    $seed | ConvertTo-Json -Depth 10 | Out-File $outFile -Encoding utf8

    Write-Host "→ Created $outFile"
}

# --- Run ---
foreach ($file in $inputFiles) {
    Convert-ScnToSeed -filePath $file
}