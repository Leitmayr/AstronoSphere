param(
    [Parameter(Mandatory=$true)]
    [string]$oldFile,

    [Parameter(Mandatory=$true)]
    [string]$newFile
)

if (!(Test-Path $oldFile)) {
    Write-Host "[ERR] oldFile not found"
    exit 1
}

if (!(Test-Path $newFile)) {
    Write-Host "[ERR] newFile not found"
    exit 1
}

# Byte-level compare
$oldBytes = [System.IO.File]::ReadAllBytes($oldFile)
$newBytes = [System.IO.File]::ReadAllBytes($newFile)

if ($oldBytes.Length -ne $newBytes.Length) {
    Write-Host "[DIFF] File size differs"
    exit 1
}

for ($i = 0; $i -lt $oldBytes.Length; $i++) {
    if ($oldBytes[$i] -ne $newBytes[$i]) {
        Write-Host "[DIFF] Byte mismatch at position $i"
        exit 1
    }
}

Write-Host "[OK] Binary identical"