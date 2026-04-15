# Ground Truth Factory Design Pattern

All AstronoFactories follow the same architecture.

This ensures simplicity and maintainability.

---

## Factory Structure

Each factory contains four components.

ScenarioLoader
GroundTruthRequestFactory
GT API Client
Exporter


---

## ScenarioLoader

Reads scenario definitions from the ObservationCatalog.

Responsibilities:

- load scenario metadata
- interpret time definitions
- prepare request parameters

---

## GroundTruthRequestFactory

Builds external API requests based on the scenario.

Example:

- Horizons API requests
- time service queries

---

## GT API Client

Handles communication with the external service.

Responsibilities:

- send requests
- retrieve data
- error handling

---

## Exporter

Transforms retrieved data into AstronoData datasets.

Responsibilities:

- canonical formatting
- metadata creation
- hash generation


