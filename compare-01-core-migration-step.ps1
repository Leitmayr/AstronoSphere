param(
    [string]$candidateFile,
    [string]$releasedFile
)

function Normalize-Core($core) {

    $normalized = @{}

    # --- Time ---
    $time = $core.Time

    $step = $null
    if ($time.Step) {
        $step = $time.Step
    }
    elseif ($time.StepDays) {
        $step = $time.StepDays
    }

    $normalized.Time = @{
        StartJD   = [double]$time.StartJD
        StopJD    = [double]$time.StopJD
        Step      = $step
        TimeScale = $time.TimeScale
    }

    # --- Observer ---
    $normalized.Observer = @{
        Type = $core.Observer.Type
        Body = $core.Observer.Body
    }

    # --- Targets ---
    $normalized.Targets = $core.Targets

    # --- Frame ---
    $normalized.Frame = @{
        Type  = $core.Frame.Type
        Epoch = $core.Frame.Epoch
    }

    # --- Corrections ---
    $normalized.Corrections = $core.Corrections

    return $normalized
}

function Compare-Hashtable($a, $b, $path="") {

    foreach ($key in $a.Keys) {

        $currentPath = "$path/$key"

        if (-not $b.ContainsKey($key)) {
            Write-Host "❌ Missing in B: $currentPath"
            continue
        }

        if ($a[$key] -is [hashtable]) {
            Compare-Hashtable $a[$key] $b[$key] $currentPath
        }
        else {
            if ($a[$key] -ne $b[$key]) {
                Write-Host "❌ Diff at $currentPath : $($a[$key]) vs $($b[$key])"
            }
        }
    }
}

# --- Load files ---
$candidate = Get-Content $candidateFile -Raw | ConvertFrom-Json
$released  = Get-Content $releasedFile  -Raw | ConvertFrom-Json

# --- Normalize ---
$coreA = Normalize-Core $candidate.Core
$coreB = Normalize-Core $released.Core

# --- Compare ---
Write-Host "🔍 Comparing Core..."
Compare-Hashtable $coreA $coreB

Write-Host "✅ Done"