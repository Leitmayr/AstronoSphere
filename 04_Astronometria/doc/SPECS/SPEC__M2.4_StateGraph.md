Hier sind die zwei Dokumente als **je ein kopierbarer md-Block**.

````md
# Motivation – Astronometria as the Key to AstronoSphere

## Status

Draft  
Context: M2.3 / M2.4 Meta Architecture Discussion  
Purpose: Capture the vision that emerged during the transition from simulation output integration to state-based computation.

---

# 1. The Moment

M2.3 marks an important return point.

Functionally, Astronometria had already been able to compute planetary positions before AstronoSphere was introduced. But the system has now returned to that capability on a completely different level.

Before AstronoSphere, Astronometria could compute results.

Now, Astronometria is becoming part of a reproducible scientific system.

The difference is fundamental:

```text
Before:
Compute position → inspect result

Now:
Experiment → GroundTruth → Simulation → State → Hash → Replay → Compare → Trust
````

This is not a circle.

It is the same path on a higher level.

---

# 2. Astronometria Becomes the Key

Astronometria is no longer merely the computation engine.

It becomes the access layer to AstronoSphere.

AstronoSphere provides:

* deterministic data structures
* certified experiments
* GroundTruth
* simulation containers
* validation infrastructure
* scientific reproducibility

Astronometria provides:

* computation
* exploration
* model comparison
* interactive understanding
* visual access to physical meaning

Together they form something stronger:

```text
AstronoSphere = truth, structure, validation
Astronometria = access, exploration, intuition
```

Astronometria becomes the key to opening AstronoSphere.

---

# 3. The New Role of Astronometria

The long-term vision is not to build another Stellarium.

Astronometria is meant to become a developer sandbox for astronomy:

* model-aware
* state-aware
* truth-aware
* validation-aware
* visually explorable

It should allow users to explore astronomy in ways that ordinary tools do not support.

Examples:

* plot Jupiter against a GAIA star background using Meeus, VSOP87, and Horizons at the same time
* visualize all Jupiter–Saturn conjunctions of the 20th century in one sky field
* compare Venus and Jupiter at different light-time-related moments:

  * observer time
  * time when light left Venus
  * time when light left Jupiter

These are not only visualizations.

They are experiments.

They show how astronomical results are generated, how physical corrections change them, and how models differ.

---

# 4. The Central Insight

Every plotted point must be backed by a reproducible state.

A point in Astronometria is not just a pixel.

It is a scientific statement.

It must be able to answer:

```text
What model produced me?
What target do I represent?
What observer was used?
What frame was used?
What time scale was used?
What correction level was applied?
What data did I produce?
Can I be replayed?
Can I be compared to GroundTruth?
Can I prove that I am still the same result?
```

This leads to the guiding principle:

```text
Every plotted position must be state-backed.
```

Or more strongly:

```text
A saved Astronometria scene is an executable scientific state, not just stored data.
```

---

# 5. The Trust Anchor

Repeated simulations create uncertainty.

Even if results look the same, the question remains:

```text
Did I really compute the exact same thing again?
```

The emerging state-machine concept eliminates this uncertainty.

Each computation state receives:

* a StateHash
* a DataHash

This means:

```text
StateHash = identity of the cause
DataHash  = identity of the effect
```

Together they provide a formal trust anchor:

```text
Same StateHash + same DataHash
= same state + same result
```

This transforms simulation from an imperative process into a reproducible scientific graph.

---

# 6. Self-Describing Scientific States

The deeper vision is that each relevant final state can eventually say:

```text
I know which path through the computation graph I represent.
I know which data I generated.
I know which GroundTruth pattern validates me.
I know how accurate I am against the selected GroundTruth.
```

This is a powerful architectural idea:

```text
State → Computation → Result → Validation → Trust
```

Astronometria therefore becomes more than an engine.

It becomes a system that can explain its own astronomical statements.

---

# 7. Scientific and Didactic Value

The goal is not only personal exploration.

AstronoSphere and Astronometria should provide scientific and didactic value.

Scientific value means:

* making model differences visible
* making physical corrections measurable
* making reproducible validation possible
* helping others understand the limits of astronomical models

Didactic value means:

* explaining astronomy through state-backed visual experiments
* showing how the sky is computed
* making abstract corrections such as light-time visible
* giving developers and learners a sandbox for real astronomical reasoning

The strongest formulation is:

```text
Astronometria should not only show the sky.
It should show how the sky is produced.
```

---

# 8. Final Vision

Astronometria becomes the interactive key to AstronoSphere.

AstronoSphere guarantees truth, structure, and reproducibility.

Astronometria makes this truth explorable.

The final ambition is not a beautiful black-box sky viewer.

It is a transparent scientific sandbox:

```text
A deterministic, replayable, model-comparable, truth-aware astronomy laboratory.
```

If successful, it will allow others not only to use astronomical results, but to understand how those results arise.

That is the extraordinary possibility.

````

