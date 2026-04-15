param(
    [string]$newExperimentFile,
    [string]$oldScenarioFile
)

Write-Host "Enriching $newExperimentFile with data from $oldScenarioFile..."

$newJson = Get-Content $newExperimentFile -Raw | ConvertFrom-Json
$oldJson = Get-Content $oldScenarioFile -Raw | ConvertFrom-Json

# --- ScenarioCitation übernehmen (1:1) ---
if ($oldJson.PSObject.Properties.Name -contains "ScenarioCitation") {
    $newJson.ScenarioCitation = $oldJson.ScenarioCitation
}
else {
    Write-Warning "ScenarioCitation not found in old file!"
}

# --- DatasetHeader übernehmen (1:1) ---
if ($oldJson.PSObject.Properties.Name -contains "DatasetHeader") {
    $newJson.DatasetHeader = $oldJson.DatasetHeader
}
else {
    Write-Warning "DatasetHeader not found in old file!"
}

# --- JSON serialisieren ---
$jsonString = $newJson | ConvertTo-Json -Depth 20

# --- 2-Space Formatierung ---
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

# --- überschreiben ---
[System.IO.File]::WriteAllLines($newExperimentFile, $formatted, [System.Text.Encoding]::UTF8)

Write-Host "Enrichment completed: $newExperimentFile"