# start-astronosphere.ps1

# === Explorer ===

Start-Process explorer.exe "/e,C:\Users\Marcu\source\repos\AstroWorkspace\AstronoSphere\80_Documentation"

Start-Process explorer.exe "/e,C:\Users\Marcu\source\repos\AstroWorkspace\AstronoSphere\AstronoData"

Start-Process explorer.exe "/e,C:\Users\Marcu\source\repos\AstroWorkspace\AstronoSphere\AstronoData\03_GroundTruth\Ephemeris\Horizons\Run"

Start-Process explorer.exe "/e,C:\Users\Marcu\source\repos\AstroWorkspace\AstronoSphere\AstronoData\03_GroundTruth\Ephemeris\Horizons\LastRun"
# === Visual Studio ===

Start-Process "C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\devenv.exe" ` 
"C:\Users\Marcu\source\repos\AstroWorkspace\AstronoSphere\AstronoSphere.slnx"

# === VS Code ===

Start-Process code -ArgumentList "C:\Users\Marcu\source\repos\AstroWorkspace\AstronoSphere\80_Documentation"

# === Windows Terminal ===

Start-Process wt.exe -ArgumentList "new-tab -d C:\Users\Marcu\source\repos\AstroWorkspace\AstronoSphere"

Start-Process wt.exe -ArgumentList "new-tab -d C:\Users\Marcu\source\repos\AstroWorkspace\AstronoSphere"

# === Firefox ===
Start-Process "C:\Program Files\Mozilla Firefox\firefox.exe"

# === Chat GPT ===
Start-Process "C:\Program Files\ChatGPT\ChatGPT.exe"

# === Beyond Compare ===
Start-Process "C:\Program Files\Beyond Compare 4\BCompare.exe"

# === Notepad++ ===
Start-Process "C:\Program Files\Notepad++\notepad++.exe"  

Start-Process "C:\Program Files\Notepad++\notepad++.exe" `
  -ArgumentList "-multiInst -nosession"
  
