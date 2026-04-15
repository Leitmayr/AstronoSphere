# AstronoSphere – Session Workflow Template

## Ziel
Maximaler Flow, saubere Verifikation, minimale Reibung

---

## 1) Zieldefinition
Fix/Validate: <RC / Feature / Pipeline-Step>  
Scope: <z.B. TS-A subset>  
Success: <konkretes Kriterium>

---

## 2) Test-Setup vorbereiten
- Run / LastRun sichtbar
- Referenzdaten (API)
- JSON Files
- CSV Outputs

---

## 3) Testfälle definieren
TestCase 1: <ScenarioID> → Erwartung  
TestCase 2: <ScenarioID> → Erwartung  

---

## 4) Verifikationsstrategie
API ↔ CSV ↔ JSON ↔ LastRun

---

## 5) Rollen
User: Test-Executor  
ChatGPT: Validator / Reviewer

---

## 6) Session-Regeln
1. Keine unklaren Tests  
2. Keine Analyse ohne validierten Befund  
3. Immer mehrere Quellen vergleichen  
4. Bei Zweifel → API prüfen  

---

## 7) Debug-Shortcut
1. API  
2. JSON  
3. CSV  
4. LastRun  

---

## 8) Abschluss
- Welche RCs sind geschlossen?
- Was ist validiert?
- Was bleibt offen?

---

## Session-Typen
A) Discovery  
B) Validation  
C) Pipeline  

