# EphemerisRegression – Teststrategie für TS-D und nachgelagerte Pipeline-Stufen

## Motivation

Mit TS-D (Mesh Tests) werden tausende bis zehntausende Referenzdatensätze erzeugt.
Diese Daten dienen der **quantitativen Validierung der VSOP-Theorie** über große
Zeitintervalle.

Gleichzeitig besteht die Ephemeriden-Pipeline in *Astronometria* aus mehreren
nachgelagerten Stufen (GE, GA sowie physikalische Korrekturen L1–L5), für die eine
vollständige Mesh-basierte Testdatenerzeugung einen extrem hohen Aufwand bedeuten
würde.

Dieses Dokument begründet die bewusste Entscheidung, **TS-D ausschließlich auf die
heliozentrisch-ekliptikale Stufe (HE)** zu beschränken und für die restliche Pipeline
ein **gezielt ausgewähltes, kleines Test-Subset** zu verwenden.

---

## 1. Rolle von TS-D

TS-D beantwortet exakt eine zentrale Frage:

> *Wie verhält sich die numerische Genauigkeit der VSOP-Theorie über Zeit, Epoche
> und Himmelskörper hinweg?*

Diese Fragestellung ist:
- modellzentriert
- zeitabhängig
- statistisch
- hochdimensional

Sie ist **vollständig beantwortet**, wenn sie auf **heliozentrisch-ekliptikale
Koordinaten (HE)** angewendet wird.

### Begründung
- VSOP ist **in HE definiert**
- HE ist der **Anfang der Ephemeriden-Pipeline**
- Alle weiteren Stufen sind deterministische Transformationen oder Korrekturen

TS-D ist daher ein **Modellvalidierungstest**, kein Pipeline-End-to-End-Test.

---

## 2. Warum TS-D nicht auf GE, GA und Korrekturen ausgedehnt wird

Eine flächige Mesh-Erweiterung von TS-D auf GE, GA sowie L1–L5 hätte folgende
Nachteile:

- massive Zunahme der Datenmenge
- hoher Rechen- und API-Aufwand
- schwer interpretierbare Abweichungen
- Vermischung von:
  - Modellfehlern (VSOP)
  - Transformationsfehlern
  - numerischen Rundungsfehlern
  - physikalischen Korrekturen mit unterschiedlichen Skalen

Dies würde die **diagnostische Qualität** der Tests verschlechtern statt verbessern.

---

## 3. Testphilosophie: „Breit vs. Tief“

Die Teststrategie folgt bewusst zwei unterschiedlichen Mustern:

### TS-D: **Breit**
- sehr viele Zeitpunkte
- große Zeitintervalle (Jahrtausende)
- statistische Auswertung
- ausschließlich HE
- Ziel: Modellgüte, Degradationsverhalten, Periodizitäten

### Nachgelagerte Pipeline-Stufen: **Tief**
- wenige, gezielt ausgewählte Zeitpunkte
- physikalisch und geometrisch kritische Situationen
- deterministisch und gut interpretierbar
- Ziel: Korrektheit der Transformationen und Korrekturen

---

## 4. Subset-Strategie für GE, GA und Korrekturen

Für die Stufen hinter HE wird **kein flächiges Mesh**, sondern ein bewusst
konstruiertes Test-Subset verwendet.

### Auswahlkriterien für Testzeitpunkte
Die Zeitpunkte werden nicht zufällig gewählt, sondern anhand physikalischer und
geometrischer Relevanz:

- Quadrantenübergänge (aus TS-A)
- auf- und absteigende Knoten (aus TS-B)
- Deklinationswechsel (aus TS-C)
- Perihel / Aphel
- Opposition / Konjunktion
- große Erdnähe (maximale Aberration / Lichtlaufzeit)

Diese Situationen sind **numerisch sensibel** und besonders geeignet, Fehler in
Transformationen und Korrekturen sichtbar zu machen.

---

## 5. Realistische Größenordnung

| Pipeline-Stufe | Anzahl Testpunkte |
|----------------|------------------|
| HE (TS-D)      | 10.000+          |
| GE             | 50–100           |
| GA             | 50–100           |
| L1–L5          | 20–50            |

Diese Größenordnung ist:
- wartbar
- rechnerisch effizient
- gut interpretierbar
- langfristig erweiterbar

---

## 6. Architektonische Vorteile

Diese Strategie führt zu:

- klarer Verantwortlichkeit der Tests
  - TS-D → Modellvalidierung
  - Subset-Tests → Pipeline-Korrektheit
- eindeutiger Fehlerzuordnung
- sauberer Trennung von Theorie, Transformation und Korrektur
- einfacher Erweiterbarkeit bei:
  - neuen Ephemeriden (z. B. DE-440)
  - neuen Korrekturen
  - neuen Pipeline-Stufen

---

## 7. Fazit

Die Beschränkung von TS-D auf die heliozentrisch-ekliptikale Stufe ist **keine
Vereinfachung**, sondern eine **bewusste, fachlich korrekte Testdesign-Entscheidung**.

> **TS-D validiert das Modell.  
> Subset-Tests validieren die Pipeline.**

Alles andere wäre Overengineering mit geringem Erkenntnisgewinn.