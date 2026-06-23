Ja. Für AstronoSphere/AstronoTruth würde ich das als technische Architektur-Doku ungefähr so beschreiben:

# AstronoTruthDiagnosticGuard

## Zweck

`AstronoTruthDiagnosticGuard` ist die zentrale Komponente zur Erzeugung von Diagnosen im AstronoTruth-Horizons-Workflow.

Die Klasse übernimmt zwei Aufgaben:

1. **Pre-Request-Validierung**

   * Prüft, ob ein Experiment grundsätzlich für einen Horizons-Aufruf geeignet ist.
   * Verhindert unnötige Provider-Anfragen bei offensichtlichen Fehlern.

2. **Fehlerdiagnostik während der Verarbeitung**

   * Erzeugt standardisierte Diagnosen für Netzwerk-, Provider- und Parsingfehler.
   * Wandelt technische Fehler in strukturierte `DiagnosticRecord`-Objekte um.

Die Klasse arbeitet ausschließlich auf dem JSON-Experimentmodell (`JsonElement`) und erzeugt keine Exceptions als fachliche Rückgabe.

---

# Verantwortlichkeiten

## 1. Experiment-Freigabe prüfen

Vor einem Horizons-Request wird geprüft:

### Maturity Status

Pfad:

```text
Metadata.Status.Maturity
```

Erwarteter Wert:

```text
Released
```

Falls ein anderer Wert vorliegt:

```text
Draft
Experimental
Deprecated
...
```

wird eine Diagnose

```text
AstronoTruthInvalidMaturity
```

erzeugt.

Ziel:

Nur offiziell freigegebene Experimente dürfen AstronoTruth-Daten erzeugen.

---

## 2. Provider-Zeitbereich prüfen

Nach erfolgreicher Maturity-Prüfung wird der Zielkörper ermittelt:

```text
Core.ObservedObject.Targets[0]
```

Anschließend werden

```text
StartJD
StopJD
```

ausgelesen.

Für das Zielobjekt wird über

```csharp
HorizonsProviderRangeCatalog.GetRange(...)
```

der zulässige Horizons-Datenbereich bestimmt.

Prüfung:

```text
StartJD >= ProviderMinJD
StopJD  <= ProviderMaxJD
```

Bei Verletzung wird eine Diagnose

```text
AstronoTruthProviderRangeViolation
```

erzeugt.

Ziel:

Verhindern von Horizons-Anfragen außerhalb des bekannten Provider-Datenbestands.

---

# Öffentliche API

## EvaluatePreRequest(...)

### Zweck

Führt alle Vorprüfungen durch.

### Ablauf

1. Maturity lesen
2. Released prüfen
3. Target bestimmen
4. Zeitbereich bestimmen
5. Provider-Grenzen prüfen

### Rückgabe

```csharp
DiagnosticRecord?
```

Bedeutung:

| Rückgabe         | Bedeutung                |
| ---------------- | ------------------------ |
| null             | Alle Prüfungen bestanden |
| DiagnosticRecord | Fehler wurde erkannt     |

Damit kann der Aufrufer unmittelbar entscheiden:

```csharp
var diag = AstronoTruthDiagnosticGuard.EvaluatePreRequest(root);

if (diag != null)
{
    return diag;
}
```

---

## BuildRequestFailed(...)

### Zweck

Erzeugt eine Diagnose für fehlgeschlagene Horizons-Requests.

Typische Ursachen:

* HTTP Fehler
* Timeout
* Netzwerkfehler
* Provider nicht erreichbar
* ungültige Request-Parameter

### Enthaltene Details

```text
Target
RequestUrl
Reason
ResponseSnippet (optional)
```

Diagnosecode:

```text
AstronoTruthRequestFailed
```

---

## BuildParseFailed(...)

### Zweck

Erzeugt eine Diagnose für Parsingfehler.

Typische Ursachen:

* Unerwartetes Horizons-Format
* Fehlende Datenblöcke
* Fehlerhafte Antwortstruktur

### Enthaltene Details

```text
Target
RequestUrl
ParseStage
Reason
```

Diagnosecode:

```text
AstronoTruthParseFailed
```

---

# Diagnose-Erzeugung

Alle Diagnosen werden über

```csharp
BuildBaseRecord(...)
```

vereinheitlicht erzeugt.

Dadurch besitzen sämtliche AstronoTruth-Diagnosen dieselbe Struktur.

---

# Standardfelder eines DiagnosticRecord

Folgende Felder werden automatisch gesetzt:

| Feld            | Wert              |
| --------------- | ----------------- |
| SourceSystem    | AstronoTruth      |
| SubSourceSystem | Horizons          |
| InputObjectType | Experiment        |
| InputObjectId   | ExperimentID      |
| CatalogNumber   | CatalogNumber     |
| CreatedAtUtc    | Aktuelle UTC-Zeit |

Zusätzlich werden die Metadaten aus der jeweiligen `DiagnosticCodeDefinition` übernommen:

```text
Code
Symbol
Severity
```

---

# Hilfsmethoden

Die Klasse enthält mehrere private JSON-Zugriffsfunktionen:

## GetCatalogNumber()

Liest:

```text
CatalogNumber
```

---

## GetExperimentId()

Liest:

```text
ExperimentID
```

---

## GetTarget()

Liest:

```text
Core.ObservedObject.Targets[0]
```

---

## GetStartJD()

Liest:

```text
Core.Time.StartJD
```

---

## GetStopJD()

Liest:

```text
Core.Time.StopJD
```

Diese Methoden kapseln die JSON-Pfade und vermeiden redundanten Zugriffscode.

---

# Architekturrolle innerhalb von AstronoTruth

```text
Experiment JSON
       │
       ▼
AstronoTruthDiagnosticGuard
       │
       ├─ Released?
       │
       ├─ Provider Range valid?
       │
       ▼
Horizons Request
       │
       ├─ Request Error
       │      ▼
       │  BuildRequestFailed()
       │
       ├─ Parse Error
       │      ▼
       │  BuildParseFailed()
       │
       ▼
AstronoTruth Result
```

Die Klasse bildet damit die erste Verteidigungslinie gegen ungültige Experimente sowie die zentrale Fabrik für alle AstronoTruth-spezifischen Diagnosen im Horizons-Zugriffspfad.

---

# Designprinzipien

## Fail Fast

Ungültige Experimente werden vor dem ersten Provider-Zugriff erkannt.

---

## Single Responsibility

Die Klasse besitzt ausschließlich Diagnose- und Validierungslogik.

Keine:

* HTTP-Kommunikation
* Parsinglogik
* Dateizugriffe
* Persistierung

---

## Konsistente Diagnostik

Alle Fehler werden als strukturierte `DiagnosticRecord`-Objekte ausgegeben.

Dadurch können:

* Logs
* Reports
* GUI-Anzeigen
* Regressionstests

auf ein einheitliches Fehlermodell zugreifen.

---

# M2.1 Einordnung

Im M2.1 AstronoDiag-Konzept stellt `AstronoTruthDiagnosticGuard` den zentralen Guard Layer vor dem Horizons Provider dar.

Er sorgt dafür, dass:

1. Nur freigegebene Experimente verarbeitet werden.
2. Nur gültige Provider-Zeitbereiche angefragt werden.
3. Request-Fehler standardisiert diagnostiziert werden.
4. Parsing-Fehler standardisiert diagnostiziert werden.

Damit bildet die Klasse die Brücke zwischen AstronoTruth und dem generischen AstronoDiag-Diagnosesystem.

Mein Architektur-Kommentar dazu: Die Klasse ist überraschend sauber. Sie ist praktisch kein "Guard" im klassischen Sinn, sondern eher eine **Diagnostic Factory + Preflight Validator**. Wenn Ihr M2.x weiter ausbaut, würde ich langfristig sogar überlegen, sie umzubenennen in:

```text
AstronoTruthDiagnostics
```

oder

```text
AstronoTruthDiagnosticFactory
```

weil mittlerweile 80 % des Codes Diagnoseerzeugung und nur 20 % echte Guard-Logik sind. Das ist kein Fehler – nur die Verantwortung der Klasse hat sich bereits etwas über den ursprünglichen Namen hinaus entwickelt.
