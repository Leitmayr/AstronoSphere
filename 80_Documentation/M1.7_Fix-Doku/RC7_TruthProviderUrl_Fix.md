# M1.7 – RC7 Fix: TruthProviderUrl

## Status
✅ RC7 erfolgreich abgeschlossen

---

## Ziel von RC7

Sicherstellen, dass die im DatasetHeader gespeicherte URL exakt dem tatsächlich ausgeführten Horizons API-Call entspricht.

---

## Problem

Vor RC7:

- TruthProviderUrl wurde aus einem „canonical request“ konstruiert
- diese URL war NICHT identisch mit dem echten API-Call
- Browser-Ausführung konnte abweichen

---

## Implementierter Fix

- URL wird direkt aus den echten Request-Parametern aufgebaut
- identische Logik wie im HorizonsApiClient

```text
TruthProviderUrl == tatsächlicher API-Call
```

---

## Ergebnis

### 1. Reproduzierbarkeit

- URL direkt im Browser ausführbar
- liefert identische Daten wie Pipeline

### 2. Konsistenz

- API == CSV == JSON
- keine Transformation oder Abweichung mehr

### 3. Traceability

- DatasetHeader beschreibt exakt den echten Request
- vollständige Nachvollziehbarkeit gewährleistet

---

## Validierung

Getestet mit mehreren Datensätzen:

- URL aus JSON kopiert
- im Browser ausgeführt
- Ergebnis mit CSV verglichen

Ergebnis:

- vollständig identisch

---

## Designentscheidung (Freeze)

Für M1.7 wird festgelegt:

- TruthProviderUrl muss immer der echte API-Call sein
- keine rekonstruierte oder vereinfachte URL
- vollständige Parametertransparenz

---

## Bedeutung

- Grundlage für Debugging
- Grundlage für wissenschaftliche Reproduzierbarkeit
- essenziell für zukünftige Provider (M2)

---

## Nächster Schritt

M1.7 Abschluss & Übergang zu Phase 3 – Horizons Mapping Refactor
