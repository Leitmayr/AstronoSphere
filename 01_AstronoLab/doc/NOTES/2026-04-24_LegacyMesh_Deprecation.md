
# Context

The new mesh generation contains better structured mesh definitions, see Reference below.
A side effect of this refactoring is that the existing Meshes (data sets #72-#142) are outdated and need to be set to "deprecated".

There are different ways to do this:

1) running the entire pipeline again
2) set experiments to status "deprecated" manually

Set GroundTruthData in 03_GroundTruth/../Baseline to "deprecated" as well.


# Key Decisions

Legacy mesh experiments #72–#142 and their GroundTruth/Baseline datasets
will be marked deprecated **manually** after the new M2.1 mesh import is complete.

No pipeline regeneration.
No EphemerisFactory behavior change.
No automated GroundTruth regeneration for deprecated experiments.

# Decision Detail

Deprecated legacy mesh files are marked by:

Metadata.Status.Maturity = "Deprecated"

Affected stages:
- GeneratedSeeds
- PreparedSeeds
- Experiments

Pipeline rule:
- AstronoCert processes only input files with Metadata.Status.Maturity = "Released"
- AstronoTruth processes only input files with Metadata.Status.Maturity = "Released"

# Definition

Deprecated:
- Experiment: Status = "deprecated"
- GroundTruth: flagged in DatasetHeader OR moved to dedicated folder

# Open Questions

- how exactly we will treat the Ground Truth data. This shall be decided in a later development step, where we eventually define the lifecycle.

# Insights
Implement new Meshes first to avoid scope drift (-> Stealth Manifest)

# Next Steps
- Implement new Meshes by means of AstornoLab/MeshGenerationRunner.cs

# Validation

- Verify deprecated flag is correctly set in Experiments and GroundTruth
- Ensure no deprecated datasets are used in pipeline runs

# References
- CORE_CanonicalMeshDefinitions.md
