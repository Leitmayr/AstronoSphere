# Astronometria -- Architecture Freeze


Version 2.0 (further development on 2026-02-14)

- Time mdoel: TT instead of UT
- Helo -> Geo to Ephemerdies and not in Projection


Version 1.0 (initial revision from 2026-01.20)

------------------------------------------------------------------------

## 1. Grundprinzip

Die Architektur trennt strikt:

-   Numerik / Ephemeriden
-   Koordinatentransformation
-   Projektion
-   Rendering
-   Datenzugriff (Parser)
-   Simulation / Ereignis-Engine

Rendering ist **orthogonal** zu Simulation und Ephemeris.

------------------------------------------------------------------------

## 2. Solution-Struktur

    Astronometria               (WPF App)
    AstroSim.Core               (Grundtypen, Zeit, Beobachter, Math)
    AstroSim.Data               (Parser, Kataloge, VSOP-Loader, Bahndaten)
    AstroSim.Ephemerides        (VSOP, Planet-Provider, später Moon etc.)
    AstroSim.Projection         (Projektionen, Mapping, Koordinatentransform)
    AstroSim.Simulation         (Event-Engine, Konjunktionen etc.)
    AstroSim.Tests              (NUnit Tests)

------------------------------------------------------------------------

## 3. Schichtenmodell

### 3.1 Data Layer

Verantwortlich für: - Bright Star Catalog Parser - Constellation Lines
Parser - VSOP Parser - Testdaten Parser - Bahndaten

Keine Logik. Nur Datenbereitstellung.

------------------------------------------------------------------------

### 3.2 Core Layer

Enthält: - Zeitmodell (JD / UT) - Beobachtungspunkt - mathematische
Hilfsklassen - Winkel-Normalisierung - Vektorrechnung

Keine GUI-Abhängigkeit.

------------------------------------------------------------------------

### 3.3 Ephemerides Layer

Verantwortlich für: - VSOP87A Berechnung (kartesisch) - PlanetId -
Klasse `VsopPlanet` - später: Mond, Spezialmodelle

Liefert: - heliozentrische XYZ in AU

Keine Projektion. Keine GUI.

------------------------------------------------------------------------

### 3.4 Projection Layer

Verantwortlich für: - Helio → Geo Transformation - Ekliptik → Äquator
Rotation - Äquator → Horizontal - Gnomonische Projektion -
Mercator-Projektion - Mapping in normierten Radius \[0..1\]

Keine Kenntnis von Rendering.

------------------------------------------------------------------------

### 3.5 Simulation Layer

Verantwortlich für: - Ereigniserkennung (Konjunktion, Opposition etc.) -
Zeititeration - Trajektorien - spätere Mond-Modelle - Event-Handler
System

Komplett unabhängig von GUI.

------------------------------------------------------------------------

### 3.6 Presentation Layer (WPF)

Verantwortlich für: - StarMapControl - DrawingVisual Rendering -
UI-Interaktion

Keine Ephemeris-Logik.

------------------------------------------------------------------------

## 4. Koordinaten-Konventionen

### VSOP87A

-   kartesische Koordinaten (X,Y,Z)
-   Einheit: AU
-   dynamisches System
-   keine Winkel-Normalisierung im Kern

### Winkelbehandlung

-   Längen: 0..360°
-   Breiten: -90..+90°
-   Keine gemeinsame Normalisierung

------------------------------------------------------------------------

## 5. Zeitmodell

-   Interne Rechnungen in UT / JD
-   Lokalzeit nur für Anzeige
-   Sommerzeit nur UI-relevant

------------------------------------------------------------------------

## 6. Teststrategie

-   NUnit
-   Unit-Tests für:
    -   VSOP
    -   Parser
    -   Transformationsfunktionen
-   Integrationstests für Helio → Geo → RA/Dec
-   Rendering nicht unit-getestet

------------------------------------------------------------------------

## 7. Performance-Strategie

-   VSOP nur bei Zeitänderung
-   Caching möglich
-   Simulation getrennt vom Rendering-Thread
-   Rendering via DrawingVisual

------------------------------------------------------------------------

## 8. Erweiterbarkeit

Architektur berücksichtigt:

-   Planeten
-   Monde
-   Kometen
-   Asteroiden
-   Trajektorien
-   Reports (HTML/XML)
-   Exportfunktionen

------------------------------------------------------------------------

## 9. Aktueller Status

-   VSOP87A neu implementiert
-   Kartesische Basis stabil
-   Tests bestehen mit hoher Genauigkeit
-   Alte VSOP87D-Struktur verworfen
-   Projektstruktur steht

------------------------------------------------------------------------

End of Architecture Freeze v1.0
