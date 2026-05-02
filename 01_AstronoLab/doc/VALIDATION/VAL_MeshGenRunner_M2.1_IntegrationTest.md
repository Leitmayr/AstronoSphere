
## Plan für die Implementierung (Bug fix Meta Data)

MeshGenerator Code fixen (Metadata.Status)

### Quick Check: Beyond Compare des Changes

Vergleich alte und neue Implementierung mit Beyond Compare. Ziel: Sicherstellen, dass lediglich die Struktur MetadataData gefixt wurde und der Rest unangetastet geblieben ist.

### Validation MeshGerator
1) Seeds/Incoming vollständig erzeugen und BeyondCompare mit LastRun machen. Wenn nur die Struktur anders -> Weiter mit
2) BeyondCompare mit altem Saturn MXT1 Seed/Incoming machen und schauen, ob Struktur jetzt wirklich passt.

MeshGenRunner Code fixen (Metadata.Status), falls erforderlich

### Validation MeshGenRunner
3) GMSS erzeugen und BeyondCompare mit LastRun machen. Wenn nur die Struktur anders -> alle 229 erzeugen.
4) BeyondCompare mit altem DS #23 machen und schauen, ob Struktur jetzt wirklich passt.

### Validation AstronoCert
5) AstronoCert erzeugen. Experiment mit Seed/Processed vergleichen. Erwarteter Unterschied: Files identisch, lediglich CoreHash berechnet in AstronoCert


### Test Protocol

#### Beyond Compare tes Changes

Ergebnis: Lediglich Struktur angepasst

#### MesGenerator
1) + 2) Saturn MXT1 alte Files wie neue, bis auf die gewünschte Änderung in Metadata

#### MeshGenRunner

3) GMSS prüfen
    - File 146: ok
    - File 206: ok
    - File 224: ok
    - File 257: ok
    - File 286: ok
    - File 343: ok
    - File 345: ok
    - File 366: ok

4) 229 File erzeugt -> ok, Struktur von Metadata in #146 identisch mit Struktur alter #23 -> ok

Darüber hinaus: Re-Run von Astronolab. Ergebnis: Run == LastRun -> ok

#### AstronoCert

5) GMSS prüfen: nur CoreHash erzeugt gegenüber Seed/Processed?
    - File 146: ok
    - File 206: ok
    - File 224: ok
    - File 257: ok
    - File 286: ok
    - File 343: ok
    - File 345: ok
    - File 366: ok

Abschlusstest: re-Run Astronocert: Experiments #146 - #374: Run==LastRun