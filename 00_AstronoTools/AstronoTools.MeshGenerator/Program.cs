using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AstronoTools.MeshGenerator;

/// <summary>
/// PURPOSE
/// Deterministically generate Mesh SeedCandidates for M2.1 based on the canonical mesh rules.
///
/// CONTEXT
/// - Simulation Mesh: MCRE, MXT1, MXT2
/// - Validation Mesh: MVH1, MVH2, MVH3
///
/// CONSTRAINTS
/// - No manual JD_Start / JD_Stop per segment
/// - ValidationMesh = SimulationMesh ∩ ProviderRange(planet)
/// - Output structure must match MeshGenerator_ExampleSeed.json
/// - Output path must be resolved via repo root, not via "..\..\..\"
/// - Within each Epoch, the mesh grid is globally continuous
/// - SubEpochs are slices only and MUST NOT reset the grid phase
/// </summary>
internal static class Program
{
    private const double Epsilon = 1e-9;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        DefaultIgnoreCondition = JsonIgnoreCondition.Never
    };

    private static int Main(string[] args)
    {
        try
        {
            var request = RunRequest.Parse(args);

            Console.WriteLine("AstronoTools.MeshGenerator");
            Console.WriteLine("--------------------------");
            Console.WriteLine($"Mode         : {request.Abbreviation}");
            Console.WriteLine($"Planet Filter: {(request.PlanetFilter ?? "ALL")}");
            Console.WriteLine();

            var allGeneratedSeeds = GenerateSeeds(request);

            PrintSummaryTable(request);

            if (allGeneratedSeeds.Count == 0)
            {
                Console.WriteLine("No seeds generated.");
                return 0;
            }

            var outputDirectory = GetIncomingSeedsPath();
            Directory.CreateDirectory(outputDirectory);

            foreach (var seed in allGeneratedSeeds)
            {
                var filePath = Path.Combine(outputDirectory, $"{seed.SeedOrigin.ResultID}.json");
                var wrapper = new GeneratedSeedsFile
                {
                    GeneratedSeeds = new List<GeneratedSeed> { seed }
                };

                var json = JsonSerializer.Serialize(wrapper, JsonOptions);
                File.WriteAllText(filePath, json);

                Console.WriteLine($"Written: {filePath}");
            }

            Console.WriteLine();
            Console.WriteLine($"Done. Generated seeds: {allGeneratedSeeds.Count}");
            return 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR");
            Console.WriteLine("-----");
            Console.WriteLine(ex.Message);
            return 1;
        }
    }

    private static List<GeneratedSeed> GenerateSeeds(RunRequest request)
    {
        var planets = GetPlanets(request.PlanetFilter);
        var generatedSeeds = new List<GeneratedSeed>();

        foreach (var meshDefinition in GetRequestedMeshDefinitions(request.Abbreviation))
        {
            foreach (var planet in planets)
            {
                foreach (var segment in BuildSegments(meshDefinition, planet))
                {
                    PrintSegment(segment);

                    if (!segment.ShouldGenerate)
                    {
                        continue;
                    }

                    generatedSeeds.Add(MapToSeed(segment, planet));
                }
            }
        }

        return generatedSeeds;
    }

    private static IEnumerable<MeshDefinition> GetRequestedMeshDefinitions(string abbreviation)
    {
        var all = GetMeshDefinitions();

        if (string.Equals(abbreviation, "ALL", StringComparison.OrdinalIgnoreCase))
        {
            return all;
        }

        var match = all.SingleOrDefault(m => string.Equals(m.Abbreviation, abbreviation, StringComparison.OrdinalIgnoreCase));
        if (match == null)
        {
            throw new InvalidOperationException(
                $"Unknown abbreviation '{abbreviation}'. Allowed: MCRE, MXT1, MXT2, MVH1, MVH2, MVH3, ALL");
        }

        return new[] { match };
    }

    private static IReadOnlyList<MeshDefinition> GetMeshDefinitions()
    {
        return new List<MeshDefinition>
        {
            new MeshDefinition(
                "MCRE",
                MeshKind.Simulation,
                1,
                30,
                1217,
                1600,
                2500,
                "1600-2500",
                new List<SubEpochDefinition>
                {
                    new SubEpochDefinition(1, 1, 1600, 1700, "Core"),
                    new SubEpochDefinition(1, 2, 1700, 1800, "Core"),
                    new SubEpochDefinition(1, 3, 1800, 1900, "Core"),
                    new SubEpochDefinition(1, 4, 1900, 2000, "Core"),
                    new SubEpochDefinition(1, 5, 2000, 2100, "Core"),
                    new SubEpochDefinition(1, 6, 2100, 2200, "Core"),
                    new SubEpochDefinition(1, 7, 2200, 2300, "Core"),
                    new SubEpochDefinition(1, 8, 2300, 2400, "Core"),
                    new SubEpochDefinition(1, 9, 2400, 2500, "Core")
                }),

            new MeshDefinition(
                "MXT1",
                MeshKind.Simulation,
                2,
                211,
                1731,
                0,
                4000,
                "0000-4000",
                new List<SubEpochDefinition>
                {
                    new SubEpochDefinition(2, 1, 0, 1000, "Extended"),
                    new SubEpochDefinition(2, 2, 1000, 2000, "Extended"),
                    new SubEpochDefinition(2, 3, 2000, 3000, "Extended"),
                    new SubEpochDefinition(2, 4, 3000, 4000, "Extended")
                }),

            new MeshDefinition(
                "MXT2",
                MeshKind.Simulation,
                3,
                809,
                1805,
                -4000,
                8000,
                "-4000-8000",
                new List<SubEpochDefinition>
                {
                    new SubEpochDefinition(3, 1, -4000, 0, "Outer"),
                    new SubEpochDefinition(3, 2, 0, 4000, "Outer"),
                    new SubEpochDefinition(3, 3, 4000, 8000, "Outer")
                }),

            new MeshDefinition(
                "MVH1",
                MeshKind.Validation,
                1,
                30,
                1217,
                1600,
                2500,
                "1600-2500",
                new List<SubEpochDefinition>
                {
                    new SubEpochDefinition(1, 1, 1600, 1700, "Core"),
                    new SubEpochDefinition(1, 2, 1700, 1800, "Core"),
                    new SubEpochDefinition(1, 3, 1800, 1900, "Core"),
                    new SubEpochDefinition(1, 4, 1900, 2000, "Core"),
                    new SubEpochDefinition(1, 5, 2000, 2100, "Core"),
                    new SubEpochDefinition(1, 6, 2100, 2200, "Core"),
                    new SubEpochDefinition(1, 7, 2200, 2300, "Core"),
                    new SubEpochDefinition(1, 8, 2300, 2400, "Core"),
                    new SubEpochDefinition(1, 9, 2400, 2500, "Core")
                }),

            new MeshDefinition(
                "MVH2",
                MeshKind.Validation,
                2,
                211,
                1731,
                0,
                4000,
                "0000-4000",
                new List<SubEpochDefinition>
                {
                    new SubEpochDefinition(2, 1, 0, 1000, "Extended"),
                    new SubEpochDefinition(2, 2, 1000, 2000, "Extended"),
                    new SubEpochDefinition(2, 3, 2000, 3000, "Extended"),
                    new SubEpochDefinition(2, 4, 3000, 4000, "Extended")
                }),

            new MeshDefinition(
                "MVH3",
                MeshKind.Validation,
                3,
                809,
                1805,
                -4000,
                8000,
                "-4000-8000",
                new List<SubEpochDefinition>
                {
                    new SubEpochDefinition(3, 1, -4000, 0, "Outer"),
                    new SubEpochDefinition(3, 2, 0, 4000, "Outer"),
                    new SubEpochDefinition(3, 3, 4000, 8000, "Outer")
                })
        };
    }

    private static IEnumerable<SegmentResult> BuildSegments(MeshDefinition meshDefinition, PlanetDefinition planet)
    {
        var globalEpochStart = ToJulianDateAtMidnight(meshDefinition.EpochStartYearInclusive, 1, 1);
        var globalEpochEndExclusive = ToJulianDateAtMidnight(meshDefinition.EpochEndYearExclusive, 1, 1);

        foreach (var subEpoch in meshDefinition.SubEpochs)
        {
            var sliceStart = ToJulianDateAtMidnight(subEpoch.StartYearInclusive, 1, 1);
            var sliceEndExclusive = ToJulianDateAtMidnight(subEpoch.EndYearExclusive, 1, 1);

            if (meshDefinition.MeshKind == MeshKind.Simulation)
            {
                yield return BuildSimulationSegment(
                    meshDefinition,
                    subEpoch,
                    planet,
                    globalEpochStart,
                    globalEpochEndExclusive,
                    sliceStart,
                    sliceEndExclusive);

                continue;
            }

            yield return BuildValidationSegment(
                meshDefinition,
                subEpoch,
                planet,
                globalEpochStart,
                globalEpochEndExclusive,
                sliceStart,
                sliceEndExclusive);
        }
    }

    private static SegmentResult BuildSimulationSegment(
        MeshDefinition meshDefinition,
        SubEpochDefinition subEpoch,
        PlanetDefinition planet,
        double globalEpochStart,
        double globalEpochEndExclusive,
        double sliceStart,
        double sliceEndExclusive)
    {
        var generatedStart = FirstAlignedOnOrAfter(globalEpochStart, meshDefinition.StepDays, sliceStart);
        var generatedStop = LastAlignedBefore(globalEpochStart, meshDefinition.StepDays, sliceEndExclusive);

        var intersects =
            generatedStart < sliceEndExclusive - Epsilon &&
            generatedStop >= sliceStart - Epsilon &&
            generatedStart <= generatedStop + Epsilon &&
            generatedStart >= globalEpochStart - Epsilon &&
            generatedStop < globalEpochEndExclusive + Epsilon;

        if (!intersects)
        {
            return new SegmentResult(
                planet,
                meshDefinition,
                subEpoch,
                sliceStart,
                sliceEndExclusive,
                null,
                null,
                false,
                SegmentValidation.Empty());
        }

        var validation = ValidateSimulationSegment(
            meshStart: globalEpochStart,
            segmentStart: generatedStart,
            segmentStop: generatedStop,
            stepDays: meshDefinition.StepDays);

        return new SegmentResult(
            planet,
            meshDefinition,
            subEpoch,
            sliceStart,
            sliceEndExclusive,
            generatedStart,
            generatedStop,
            true,
            validation);
    }

    private static SegmentResult BuildValidationSegment(
        MeshDefinition meshDefinition,
        SubEpochDefinition subEpoch,
        PlanetDefinition planet,
        double globalEpochStart,
        double globalEpochEndExclusive,
        double sliceStart,
        double sliceEndExclusive)
    {
        var providerRange = planet.HorizonsRange;

        var firstAlignedInSlice = FirstAlignedOnOrAfter(globalEpochStart, meshDefinition.StepDays, sliceStart);
        var lastAlignedInSlice = LastAlignedBefore(globalEpochStart, meshDefinition.StepDays, sliceEndExclusive);

        var generatedStart = FirstAlignedOnOrAfter(
            globalEpochStart,
            meshDefinition.StepDays,
            Math.Max(sliceStart, providerRange.MinJD));

        var generatedStop = LastAlignedOnOrBefore(
            globalEpochStart,
            meshDefinition.StepDays,
            Math.Min(lastAlignedInSlice, providerRange.MaxJD));

        var intersects =
            firstAlignedInSlice < sliceEndExclusive - Epsilon &&
            lastAlignedInSlice >= sliceStart - Epsilon &&
            generatedStart <= generatedStop + Epsilon &&
            generatedStart <= lastAlignedInSlice + Epsilon &&
            generatedStop >= firstAlignedInSlice - Epsilon &&
            generatedStart >= globalEpochStart - Epsilon &&
            generatedStop < globalEpochEndExclusive + Epsilon;

        if (!intersects)
        {
            return new SegmentResult(
                planet,
                meshDefinition,
                subEpoch,
                sliceStart,
                sliceEndExclusive,
                null,
                null,
                false,
                SegmentValidation.Empty());
        }

        var validation = ValidateValidationSegment(
            meshStart: globalEpochStart,
            segmentStart: generatedStart,
            segmentStop: generatedStop,
            stepDays: meshDefinition.StepDays,
            providerMin: providerRange.MinJD,
            providerMax: providerRange.MaxJD);

        return new SegmentResult(
            planet,
            meshDefinition,
            subEpoch,
            sliceStart,
            sliceEndExclusive,
            generatedStart,
            generatedStop,
            true,
            validation);
    }

    private static SegmentValidation ValidateSimulationSegment(
        double meshStart,
        double segmentStart,
        double segmentStop,
        int stepDays)
    {
        var gridOk = ValidateAllGridPoints(segmentStart, segmentStop, meshStart, stepDays);
        var sampleRuleOk = CountSteps(segmentStart, segmentStop, stepDays) < 2000;

        return new SegmentValidation(
            gridOk,
            true,
            true,
            sampleRuleOk);
    }

    private static SegmentValidation ValidateValidationSegment(
        double meshStart,
        double segmentStart,
        double segmentStop,
        int stepDays,
        double providerMin,
        double providerMax)
    {
        var gridOk = ValidateAllGridPoints(segmentStart, segmentStop, meshStart, stepDays);
        var rangeOk = segmentStart >= providerMin - Epsilon && segmentStop <= providerMax + Epsilon;
        var maximalityOk = segmentStop + stepDays > providerMax - Epsilon;
        var sampleRuleOk = CountSteps(segmentStart, segmentStop, stepDays) < 2000;

        return new SegmentValidation(
            gridOk,
            rangeOk,
            maximalityOk,
            sampleRuleOk);
    }

    private static bool ValidateAllGridPoints(double start, double stop, double meshStart, int stepDays)
    {
        var stepCount = CountSteps(start, stop, stepDays);

        for (var i = 0; i <= stepCount; i++)
        {
            var current = start + (i * stepDays);
            if (!IsGridAligned(current, meshStart, stepDays))
            {
                return false;
            }
        }

        return true;
    }

    private static int CountSteps(double start, double stop, int stepDays)
    {
        return (int)Math.Round((stop - start) / stepDays);
    }

    private static int CountSamples(double start, double stop, int stepDays)
    {
        return CountSteps(start, stop, stepDays) + 1;
    }

    private static bool IsGridAligned(double value, double meshStart, int stepDays)
    {
        var offset = value - meshStart;
        var remainder = offset % stepDays;
        return Math.Abs(remainder) < Epsilon || Math.Abs(remainder - stepDays) < Epsilon;
    }

    private static double FirstAlignedOnOrAfter(double meshStart, int stepDays, double value)
    {
        if (value <= meshStart + Epsilon)
        {
            return meshStart;
        }

        var delta = value - meshStart;
        var steps = (int)Math.Ceiling(delta / stepDays);
        return meshStart + (steps * stepDays);
    }

    private static double LastAlignedOnOrBefore(double meshStart, int stepDays, double value)
    {
        if (value < meshStart - Epsilon)
        {
            return meshStart - stepDays;
        }

        var delta = value - meshStart;
        var steps = (int)Math.Floor(delta / stepDays);
        return meshStart + (steps * stepDays);
    }

    private static double LastAlignedBefore(double meshStart, int stepDays, double exclusiveValue)
    {
        return LastAlignedOnOrBefore(meshStart, stepDays, exclusiveValue - Epsilon);
    }

    private static GeneratedSeed MapToSeed(SegmentResult segment, PlanetDefinition planet)
    {
        if (!segment.GeneratedStart.HasValue || !segment.GeneratedStop.HasValue)
        {
            throw new InvalidOperationException("Cannot map empty segment to seed.");
        }

        var qualifier = BuildQualifier(
            segment.MeshDefinition.MeshKind,
            segment.SubEpoch,
            segment.SubEpoch.ScopeLabel);

        var startId = ((int)Math.Truncate(segment.GeneratedStart.Value)).ToString();
        var stopId = ((int)Math.Truncate(segment.GeneratedStop.Value)).ToString();
        var resultId =
            $"Planet-{planet.Name}-{segment.MeshDefinition.Abbreviation}_HELIO-J2000-TDB-{startId}-{stopId}-TDB_{qualifier}";

        return new GeneratedSeed
        {
            SeedCandidate = new SeedCandidate
            {
                Event = new EventData
                {
                    Category = "Mesh",
                    Qualifier = qualifier,
                    Description = segment.MeshDefinition.Abbreviation
                },
                Core = new CoreData
                {
                    Time = new TimeData
                    {
                        StartJD = segment.GeneratedStart.Value,
                        StopJD = segment.GeneratedStop.Value,
                        Step = $"{segment.MeshDefinition.StepDays}D",
                        TimeScale = "TDB"
                    },
                    Observer = new ObserverData
                    {
                        Type = "Heliocentric",
                        Body = "Sun"
                    },
                    ObservedObject = new ObservedObjectData
                    {
                        BodyClass = "Planet",
                        Targets = new List<string> { planet.Name }
                    },
                    Frame = new FrameData
                    {
                        Type = "HelioEcliptic",
                        Epoch = "J2000"
                    }
                },
                Metadata = new MetadataData
                {
                    Author = "MeshGenerator",
                    Priority = 1,
                    Maturity = "Released",
                    Visibility = "Private"
                },
                Notes = "MeshGenerator: Generated automatically based on CanonicalMeshDefinitions.md."
            },
            SeedOrigin = new SeedOriginData
            {
                ResultID = resultId,
                Reason = "Extension of the data set in M2.1",
                Trigger = "M2.1 Mesh Seed Generation",
                CreatedAtUtc = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            }
        };
    }

    private static string BuildQualifier(MeshKind meshKind, SubEpochDefinition subEpoch, string scopeLabel)
    {
        var kind = meshKind == MeshKind.Simulation ? "Simulation" : "Validation";
        return $"{kind}_SubEpoch{subEpoch.EpochNumber}.{subEpoch.SubEpochNumber}_{scopeLabel}";
    }

    private static void PrintSegment(SegmentResult segment)
    {
        Console.WriteLine($"Planet   : {segment.Planet.Name}");
        Console.WriteLine($"Mesh     : {segment.MeshDefinition.Abbreviation}");
        Console.WriteLine($"SubEpoch : {segment.SubEpoch.EpochNumber}.{segment.SubEpoch.SubEpochNumber}");

        if (!segment.ShouldGenerate)
        {
            Console.WriteLine();
            Console.WriteLine("JD_Start: -");
            Console.WriteLine("JD_Stop : -");
            Console.WriteLine();
            Console.WriteLine("Validation:");
            Console.WriteLine("  GridAlignment: -");
            Console.WriteLine("  Range        : -");
            Console.WriteLine("  Maximality   : -");
            Console.WriteLine("  SampleRule   : -");
            Console.WriteLine();
            return;
        }

        Console.WriteLine();
        Console.WriteLine($"JD_Start: {segment.GeneratedStart!.Value:F6}");
        Console.WriteLine($"JD_Stop : {segment.GeneratedStop!.Value:F6}");
        Console.WriteLine($"Samples : {CountSamples(segment.GeneratedStart.Value, segment.GeneratedStop.Value, segment.MeshDefinition.StepDays)}");
        Console.WriteLine();

        Console.WriteLine("Validation:");
        Console.WriteLine($"  GridAlignment: {ToOk(segment.Validation.GridAlignmentOk)}");
        Console.WriteLine($"  Range        : {ToOk(segment.Validation.RangeOk)}");
        Console.WriteLine($"  Maximality   : {ToOk(segment.Validation.MaximalityOk)}");
        Console.WriteLine($"  SampleRule   : {ToOk(segment.Validation.SampleRuleOk)}");
        Console.WriteLine();
    }

    private static void PrintSummaryTable(RunRequest request)
    {
        var meshDefinitions = GetRequestedMeshDefinitions(request.Abbreviation);
        var planets = GetPlanets(request.PlanetFilter);

        foreach (var mesh in meshDefinitions)
        {
            //if (mesh.MeshKind != MeshKind.Validation)
           // {
            //    continue;
            //}

            foreach (var planet in planets)
            {
                Console.WriteLine();
                Console.WriteLine($"=== TABLE: {mesh.Abbreviation} | {mesh.MeshKind} | Planet: {planet.Name} ===");
                Console.WriteLine();
                Console.WriteLine("Sub-Epoch   | Range     |  JD_Start | JD_Stop      | UTC_Start |  UTC_Stop");
                Console.WriteLine("------------|---------  | --------- | ---------    |---------  |  --------");

                var segments = BuildSegments(mesh, planet).ToList();

                foreach (var segment in segments)
                {
                    var label = $"SubEpoch{segment.SubEpoch.EpochNumber}.{segment.SubEpoch.SubEpochNumber}";
                    var range = $"{segment.SubEpoch.StartYearInclusive}-{segment.SubEpoch.EndYearExclusive}";

                    if (!segment.ShouldGenerate)
                    {
                        Console.WriteLine($"{label,-11} | {range,-9} |     -     |     -        |     -     |      -");
                        continue;
                    }

                    var jdStart = $"{segment.GeneratedStart!.Value:F1}";
                    var jdStop = $"{segment.GeneratedStop!.Value:F1}";
                    var utcStart = JDToUtcString(segment.GeneratedStart.Value);
                    var utcStop = JDToUtcString(segment.GeneratedStop.Value);

                    Console.WriteLine(
                        $"{label,-11} | {range,-9} | {jdStart,9} | {jdStop,9}    | {utcStart,9} |  {utcStop}");
                }
            }
        }
    }

    private static string JDToUtcString(double jd)
    {
        var (year, month, day) = JulianToDate(jd);
        return $"{day}.{month}.{year}";
    }

    private static (int year, int month, int day) JulianToDate(double jd)
    {
        var z = (int)Math.Floor(jd + 0.5);
        var f = (jd + 0.5) - z;

        int a;
        if (z < 2299161)
        {
            a = z;
        }
        else
        {
            var alpha = (int)((z - 1867216.25) / 36524.25);
            a = z + 1 + alpha - alpha / 4;
        }

        var b = a + 1524;
        var c = (int)((b - 122.1) / 365.25);
        var d = (int)(365.25 * c);
        var e = (int)((b - d) / 30.6001);

        var day = b - d - (int)(30.6001 * e) + (int)f;
        var month = e < 14 ? e - 1 : e - 13;
        var year = month > 2 ? c - 4716 : c - 4715;

        return (year, month, day);
    }

    private static string ToOk(bool value) => value ? "OK" : "FAIL";

    private static List<PlanetDefinition> GetPlanets(string? planetFilter)
    {
        var planets = new List<PlanetDefinition>
        {
            new PlanetDefinition("Mercury", new ProviderRange(0.5, 5373482.5)),
            new PlanetDefinition("Venus",   new ProviderRange(0.5, 5373482.5)),
            new PlanetDefinition("Earth",   new ProviderRange(0.5, 5373482.5)),
            new PlanetDefinition("Mars",    new ProviderRange(2305448.5, 2670690.5)),
            new PlanetDefinition("Jupiter", new ProviderRange(2305457.5, 2524601.5)),
            new PlanetDefinition("Saturn",  new ProviderRange(2360233.5, 2542859.5)),
            new PlanetDefinition("Uranus",  new ProviderRange(2305451.5, 2597625.5)),
            new PlanetDefinition("Neptune", new ProviderRange(2305451.5, 2597641.5))
        };

        if (string.IsNullOrWhiteSpace(planetFilter))
        {
            return planets;
        }

        var match = planets.SingleOrDefault(p => string.Equals(p.Name, planetFilter, StringComparison.OrdinalIgnoreCase));
        if (match == null)
        {
            throw new InvalidOperationException(
                $"Unknown planet '{planetFilter}'. Allowed: Mercury, Venus, Earth, Mars, Jupiter, Saturn, Uranus, Neptune");
        }

        return new List<PlanetDefinition> { match };
    }

    private static double ToJulianDateAtMidnight(int year, int month, int day)
    {
        var a = (14 - month) / 12;
        var y = year + 4800 - a;
        var m = month + (12 * a) - 3;

        var jdn =
            day +
            ((153 * m + 2) / 5) +
            (365 * y) +
            (y / 4) -
            (y / 100) +
            (y / 400) -
            32045;

        return jdn - 0.5;
    }

    public static string GetAstronoDataRoot()
    {
        return Path.Combine(GetRepoRoot(), "AstronoData");
    }

    private static string GetIncomingSeedsPath()
    {
        return Path.Combine(GetAstronoDataRoot(), "01_Seeds", "Incoming");
    }

    private static string GetRepoRoot()
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);

        while (current != null)
        {
            var astronoDataPath = Path.Combine(current.FullName, "AstronoData");
            var toolsPath = Path.Combine(current.FullName, "00_AstronoTools");

            if (Directory.Exists(astronoDataPath) && Directory.Exists(toolsPath))
            {
                return current.FullName;
            }

            current = current.Parent;
        }

        throw new DirectoryNotFoundException(
            "Could not locate AstronoSphere repository root. Expected folders 'AstronoData' and '00_AstronoTools'.");
    }

    private sealed record RunRequest(string Abbreviation, string? PlanetFilter)
    {
        public static RunRequest Parse(string[] args)
        {
            if (args.Length == 0)
            {
                return new RunRequest("MXT1", "Saturn");
            }

            if (args.Length == 1)
            {
                if (string.Equals(args[0], "ALL", StringComparison.OrdinalIgnoreCase))
                {
                    return new RunRequest("ALL", null);
                }

                return new RunRequest(args[0], "Saturn");
            }

            return new RunRequest(args[0], args[1]);
        }
    }

    private sealed record MeshDefinition(
        string Abbreviation,
        MeshKind MeshKind,
        int EpochNumber,
        int StepDays,
        int NSteps,
        int EpochStartYearInclusive,
        int EpochEndYearExclusive,
        string RangeLabel,
        List<SubEpochDefinition> SubEpochs);

    private sealed record SubEpochDefinition(
        int EpochNumber,
        int SubEpochNumber,
        int StartYearInclusive,
        int EndYearExclusive,
        string ScopeLabel);

    private sealed record PlanetDefinition(
        string Name,
        ProviderRange HorizonsRange);

    private sealed record ProviderRange(
        double MinJD,
        double MaxJD);

    private sealed record SegmentResult(
        PlanetDefinition Planet,
        MeshDefinition MeshDefinition,
        SubEpochDefinition SubEpoch,
        double SliceStart,
        double SliceEndExclusive,
        double? GeneratedStart,
        double? GeneratedStop,
        bool ShouldGenerate,
        SegmentValidation Validation);

    private sealed record SegmentValidation(
        bool GridAlignmentOk,
        bool RangeOk,
        bool MaximalityOk,
        bool SampleRuleOk)
    {
        public static SegmentValidation Empty() => new(false, false, false, false);
    }

    private enum MeshKind
    {
        Simulation,
        Validation
    }
}

public sealed class GeneratedSeedsFile
{
    public List<GeneratedSeed> GeneratedSeeds { get; set; } = new();
}

public sealed class GeneratedSeed
{
    public SeedCandidate SeedCandidate { get; set; } = new();
    public SeedOriginData SeedOrigin { get; set; } = new();
}

public sealed class SeedCandidate
{
    public EventData Event { get; set; } = new();
    public CoreData Core { get; set; } = new();
    public MetadataData Metadata { get; set; } = new();
    public string Notes { get; set; } = string.Empty;
}

public sealed class EventData
{
    public string Category { get; set; } = string.Empty;
    public string Qualifier { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public sealed class CoreData
{
    public TimeData Time { get; set; } = new();
    public ObserverData Observer { get; set; } = new();
    public ObservedObjectData ObservedObject { get; set; } = new();
    public FrameData Frame { get; set; } = new();
}

public sealed class TimeData
{
    public double StartJD { get; set; }
    public double StopJD { get; set; }
    public string Step { get; set; } = string.Empty;
    public string TimeScale { get; set; } = string.Empty;
}

public sealed class ObserverData
{
    public string Type { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}

public sealed class ObservedObjectData
{
    public string BodyClass { get; set; } = string.Empty;
    public List<string> Targets { get; set; } = new();
}

public sealed class FrameData
{
    public string Type { get; set; } = string.Empty;
    public string Epoch { get; set; } = string.Empty;
}

public sealed class MetadataData
{
    public string Author { get; set; } = string.Empty;
    public int Priority { get; set; }
    public string Maturity { get; set; } = string.Empty;
    public string Visibility { get; set; } = string.Empty;
}

public sealed class SeedOriginData
{
    public string ResultID { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public string Trigger { get; set; } = string.Empty;
    public string CreatedAtUtc { get; set; } = string.Empty;
}