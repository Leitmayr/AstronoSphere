# AstronoSphere – Getting Started

Version: 2.0  
Audience: Developers entering the system

---

# 1. What You Are Looking At

AstronoSphere is **not an astronomy application**.

It is a **scientific validation system for astronomical computation**.

The system exists to answer one question:

> Can we trust the results produced by an astronomical engine?

Everything in AstronoSphere is built around this principle.

---

# 2. The Core Idea

The system is based on one fundamental concept:

> A Scenario is a physical experiment.

A Scenario describes a real astronomical situation.

---

# 3. The Three-System Architecture

AstronoSphere separates responsibilities into three systems:

Astronometria (Engine)  
AstronoData (Data Backbone)  
AstronoFactories (Truth Layer)

---

# 4. The Validation Pipeline

Scenario
→ Reference Data (Factory)
→ Astronometria Computation
→ Comparison
→ Validation Result

---

# 5. Scenario vs Dataset

Scenario = physical experiment  
Dataset = measurement of the scenario  

---

# 6. Determinism

Same input → identical output

- canonical serialization  
- byte-level regression  
- no hidden behavior  

---

# 7. Validation Rules

PASS only if:

- structure identical  
- byte-identical  
- within tolerance  

---

# 8. Time System

Astro domain uses TT only.

---

# 9. Architectural Constraints

- Scenario Core immutable  
- no shared logic Engine / Factory  

---

# 10. How to Start

1. Understand Scenario concept  
2. Understand pipeline  
3. Follow one scenario end-to-end  

---

End of document.
