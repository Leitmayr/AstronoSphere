$released = Get-ChildItem ".\AstronoData\02_Experiments\Released" -Filter "*.json"
$gtJson   = Get-ChildItem ".\AstronoData\03_GroundTruth\Ephemeris\Horizons\Run" -Filter "*.json"
$diagJson = Get-ChildItem ".\AstronoData\03_GroundTruth\DiagMessages\Run" -Filter "*.json"

$gtCatalogs = $gtJson | ForEach-Object {
    $json = Get-Content $_.FullName -Raw | ConvertFrom-Json
    $json.ExperimentRef.CatalogNumber
}

$diagCatalogs = $diagJson | ForEach-Object {
    $json = Get-Content $_.FullName -Raw | ConvertFrom-Json
    $json.CatalogNumber
}

$outcomeCatalogs = @($gtCatalogs + $diagCatalogs) | Sort-Object -Unique

$releasedCatalogs = $released | ForEach-Object {
    $json = Get-Content $_.FullName -Raw | ConvertFrom-Json
    $json.CatalogNumber
}

$missing = $releasedCatalogs | Where-Object { $_ -notin $outcomeCatalogs }

"Released: $($releasedCatalogs.Count)"
"GroundTruth JSON: $($gtCatalogs.Count)"
"Diag JSON: $($diagCatalogs.Count)"
"Outcomes unique: $($outcomeCatalogs.Count)"
"Missing:"
$missing