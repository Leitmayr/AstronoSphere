# M1.7 – RC3 Fix: JSON Precision

## Status
✅ RC3 erfolgreich abgeschlossen

---

## Ziel von RC3

Behebung von Precision-Verlusten in der JSON-Ausgabe.

Problem:
- Numerische Werte (JD, Position, Velocity) wurden unnötig gerundet
- Ursache war die Formatierung im DatasetBuilder (`0.########`)

---

## Implementierter Fix

Änderung der Serialisierung:

- Verwendung von:
  - `G17` Format (volle double-Präzision)
- Keine künstliche Rundung mehr

```csharp
v.ToString("G17", CultureInfo.InvariantCulture);
```

---

## Ergebnis

### 1. Numerische Integrität

- volle double-Präzision erhalten
- keine Informationsverluste mehr

### 2. Konsistenz

- CSV == JSON
- API == JSON
- Run == LastRun

### 3. Determinismus

- Idempotenz bestätigt
- Byte-Level Stabilität erreicht

---

## Validierung (Testprotokoll)

Durchgeführt mit:

- TS-A #1: AS-000015
- TS-B #2: AS-000057
- TS-C #3: AS-000072

Ergebnisse:

- JSON zeigt erhöhte Präzision (erwartet)
- Werte exakt konsistent mit Horizons API
- Keine physikalisch unplausiblen Abweichungen
- Unterschiede ausschließlich durch Zeitauflösung erklärbar

---

## Wichtige Erkenntnis

Die Pipeline war korrekt.

Der Fehler lag ausschließlich in der Darstellung:

> Die Pipeline war richtig – die Serialisierung war falsch.

---

## Designentscheidung (Freeze)

Für M1.7 wird festgelegt:

- JSON verwendet volle double-Präzision (G17)
- CSV entspricht exakt dem Raw Horizons Response
- Keine Rundung im System
- JD bleibt double (keine String-Konvertierung)

---

## Auswirkungen

- Grundlage für stabile Referenzdaten
- Voraussetzung für Byte-Level Regression
- Keine versteckten numerischen Artefakte mehr

---

## Nächster Schritt

RC7 – URL Validierung

Ziel:
- URL im DatasetHeader direkt ausführbar
- 1:1 identisch mit tatsächlichem API-Call
