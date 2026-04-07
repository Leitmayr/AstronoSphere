param(
    [Parameter(Mandatory=$true)]
    [string]$oldFile,

    [Parameter(Mandatory=$true)]
    [string]$newFile
)

# ------------------------------------------------------------
# Guard Clauses
# ------------------------------------------------------------
if (!(Test-Path $oldFile)) {
    Write-Host "[ERR] oldFile not found: $oldFile"
    exit 1
}

if (!(Test-Path $newFile)) {
    Write-Host "[ERR] newFile not found: $newFile"
    exit 1
}

# ------------------------------------------------------------
# Load JSON
# ------------------------------------------------------------
$oldJson = Get-Content $oldFile -Raw | ConvertFrom-Json
$newJson = Get-Content $newFile -Raw | ConvertFrom-Json

if ($null -eq $oldJson.Core -or $null -eq $newJson.Core) {
    Write-Host "[ERR] Core missing in one of the files"
    exit 1
}

# ------------------------------------------------------------
# Normalize Core (StepDays -> Step)
# ------------------------------------------------------------
function Normalize-Core($core) {

    $step = $null
    if ($core.Time.Step) {
        $step = $core.Time.Step
    }
    elseif ($core.Time.StepDays) {
        $step = $core.Time.StepDays
    }

    return @{
        Time = @{
            StartJD   = [double]$core.Time.StartJD
            StopJD    = [double]$core.Time.StopJD
            Step      = $step
            TimeScale = $core.Time.TimeScale
        }
        Observer = @{
            Type = $core.Observer.Type
            Body = $core.Observer.Body
        }
        Targets = @($core.Targets)
        Frame = @{
            Type  = $core.Frame.Type
            Epoch = $core.Frame.Epoch
        }
        Corrections = @{
            LightTime  = [bool]$core.Corrections.LightTime
            Aberration = [bool]$core.Corrections.Aberration
            Precession = [bool]$core.Corrections.Precession
            Nutation   = [bool]$core.Corrections.Nutation
        }
    }
}

# ------------------------------------------------------------
# Deep Compare (deterministic, simple)
# ------------------------------------------------------------
function Compare-Object($a, $b, $path="") {

    # Null handling
    if ($null -eq $a -and $null -eq $b) { return }
    if ($null -eq $a -or $null -eq $b) {
        Write-Host "[DIFF] Null mismatch at $path"
        return
    }

    # Array handling
    if ($a -is [System.Array]) {
        if ($a.Count -ne $b.Count) {
            Write-Host "[DIFF] Array length mismatch at $path"
            return
        }

        for ($i = 0; $i -lt $a.Count; $i++) {
            Compare-Object $a[$i] $b[$i] "$path[$i]"
        }
        return
    }

    # Hashtable handling
    if ($a -is [hashtable]) {

        foreach ($key in $a.Keys) {

            $currentPath = "$path/$key"

            if (-not $b.ContainsKey($key)) {
                Write-Host "[DIFF] Missing in new: $currentPath"
                continue
            }

            Compare-Object $a[$key] $b[$key] $currentPath
        }

        return
    }

    # Primitive compare
    if ($a -ne $b) {
        Write-Host "[DIFF] $path : $a vs $b"
    }
}

# ------------------------------------------------------------
# Execute
# ------------------------------------------------------------
$coreOld = Normalize-Core $oldJson.Core
$coreNew = Normalize-Core $newJson.Core

Write-Host "Comparing Core..."
Compare-Object $coreOld $coreNew

Write-Host "[OK] Done"