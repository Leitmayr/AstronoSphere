## Rauschanalyse
- Quantisierungsrauschen
- Ableitungen v, a
- DE440 Terme identifizieren 
- Vergleich des Rauschverhaltens Miriade und Horizons
- Ideale Abtastzeitpunkte durch verschachtelte Zeitreihe 

##  Garbage Collector/ErrorHandler
- die einzelnen GUIs erzeugen Datenelemente und prüfen auf Probleme
- Probleme z.B. Out Of Bounds, too many Data, inconsistent Data, …
- Kategorisierung: ERROR, WARNUNG, INFO abh. Von Schwere
- Zentraler Sammler/Logger für alle erzeugten oder nicht erzeugten Files zum späteren Debugging
- zentrale Schnittstelle über AstronoData.IO?

## LifecycleDefinition
- Umgang mit deprecated Files definieren
- Dokumentieren, dass AstronoCert und AstronoTruth nur "Released" files verarbeiten (Metadata.Status.Maturity = "Released"). Gehört ins DataModel-Dokument

