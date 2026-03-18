🔹 Phase 1 – EphemerisRegression (Daten-Erzeugung)
A) Architektur sauber festgelegt

TS-B komplett in EphemerisRegression erzeugt

erst danach Übernahme ins Pipeline-Projekt

Geo und Helio gespiegelt

exakt dieselbe Struktur

keine neuen Typ-Experimente

B) RAW Export (Horizons)

Erzeugt:

Horizons/
  Geo/TS-B/Raw/
  Helio/TS-B/Raw/


Pro Planet:

<Planet>_TS-B_L0_AscendingNode.csv
<Planet>_TS-B_L0_DescendingNode.csv


Fenster:

JD_event ± 1 Tag
StepSize = 1h
RefPlane = ECLIPTIC
Correction = null


Ergebnis:
✔ 2 CSV pro Planet
✔ 7 Planeten
✔ Geo und Helio vollständig

C) JSON Export

Struktur:

Horizons/
  Geo/TS-B/Json/
  Helio/TS-B/Json/


Dateiname:

<Planet>_TS-B_L0_GeoNodes.json
<Planet>_TS-B_L0_HelioNodes.json


JSON Struktur:

{
  "Planet": "...",
  "TestSuite": "TS-B",
  "CorrectionLevel": "L0_Geo / L0_Helio",
  "Ascending": {
      "JulianDate": ...,
      "Before": { JD, X,Y,Z,VX,VY,VZ },
      "At":     { ... },
      "After":  { ... }
  },
  "Descending": { ... }
}


✔ exakt das gewünschte Format
✔ ±1d Struktur korrekt
✔ Z wechselt sauber Vorzeichen

🔹 Phase 2 – AstroSim.Ephemerides.Test (Pipeline)
A) JSON korrekt eingebunden
EphemerisValidation/TestData/
  Geo/TS-B
  Helio/TS-B


Copy to Output Directory → fix

B) Node Regression Tests

Implementiert:

GeoNodes_TS_B_L0_Tests

HelioNodes_TS_B_L0_Tests

Geprüft wird:

Z(before)
Z(at)
Z(after)


Mit planetenabhängigen Toleranzen aus:

RegressionTolerances.cs


Struktur konsistent mit TS-A.

C) Deviation Analysis

Separate Tests erzeugen:

GeoNodes_TS_B_Deviation_Statistics.csv
HelioNodes_TS_B_Deviation_Statistics.csv


Keine Typ-Duplikate mehr
Common enthält nur eine NodeEvent / NodeReferenceModel Definition
Mehrdeutigkeiten aufgelöst

📊 Ergebnisanalyse (wichtig!)
GEO

Innere Planeten:
~10⁻⁷ bis 10⁻⁶ AU
→ absolut plausibel

Gasplaneten:

Jupiter: ~8e-6

Saturn: ~1e-5

Uranus: ~1e-4

Neptune: ~3e-4

Das ist typisch für:
→ geozentrische Differenzbildung
→ große Distanzen
→ numerische Subtraktion zweier großer Vektoren

Kein Ausreißer.

HELIO

Deutlich kleiner:

Innere Planeten:
~10⁻⁷ AU

Gasplaneten:

Jupiter ~8e-6

Saturn ~1e-5

Uranus ~1e-4

Neptune ~3e-4

Fast identisch zu Geo bei äußeren Planeten → logisch:
VSOP selbst ist dort der dominante Fehlerterm.

🎯 Plausibilitätsfazit

✔ Keine numerischen Instabilitäten
✔ Keine falsche Subtraktion
✔ Z-Vorzeichen korrekt
✔ VSOP Genauigkeitsbereich konsistent
✔ Helio < Geo bei inneren Planeten (erwartet)
✔ äußere Planeten dominieren Modellfehler (erwartet)

TS-B ist physikalisch und numerisch konsistent.

🏁 Ergebnis des Tages

Wir haben:

komplette TS-B Datenpipeline aufgebaut

Geo + Helio gespiegelt

RAW + JSON sauber erzeugt

Regression Tests integriert

Deviation Analyse implementiert

Toleranzkonzept zentralisiert

Mehrdeutigkeitschaos aufgelöst

Ergebnisse plausibilisiert

Und das alles in einem sehr komplexen Code-Refactoring-Tag.