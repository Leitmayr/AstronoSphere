using EphemerisRegression.Domain;
using EphemerisRegression.Runner;
using System;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Astronometria Ephemeris Regression Generator");
        Console.WriteLine("=============================================");
        Console.WriteLine();

        if (args.Length < 2 ||
            !args[0].Equals("generate", StringComparison.OrdinalIgnoreCase))
        {
            PrintUsage();
            return;
        }

        var target = args[1].ToUpperInvariant();

        try
        {
            switch (target)
            {
                case "TS-A":
                    await GenerateTsA();
                    break;

                case "TS-B":
                    await GenerateTsB();
                    break;

                case "TS-C":
                    await GenerateTsC();
                    break;

                case "TS-C-QUICK":
                    // optional: dotnet run -- generate TS-C-QUICK Neptune
                    await QuickTestTsC(args.Length >= 3 ? args[2] : null);
                    break;

                case "TS-D":
                    await GenerateTsD(args.Length >= 3 ? args[2] : null);
                    break;

                case "TS-D-EARTH":
                    await GenerateTsDEarthQuick();
                    break;

                default:
                    Console.WriteLine($"Unknown target: {target}");
                    PrintUsage();
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR:");
            Console.WriteLine(ex);
        }

        Console.WriteLine();
        Console.WriteLine("Done.");
    }

    private static void PrintUsage()
    {
        Console.WriteLine("Usage:");
        Console.WriteLine("  dotnet run -- generate TS-A");
        Console.WriteLine("  dotnet run -- generate TS-B");
        Console.WriteLine("  dotnet run -- generate TS-C");
        Console.WriteLine("  dotnet run -- generate TS-C-QUICK");
        Console.WriteLine("  dotnet run -- generate TS-C-QUICK Neptune");
        Console.WriteLine("  dotnet run -- generate TS-D");
        Console.WriteLine("  dotnet run -- generate TS-D Earth,Mars");
        Console.WriteLine("  dotnet run -- generate TS-D-EARTH");
        Console.WriteLine();
        Console.WriteLine("Known planets: " +
            string.Join(", ", PlanetCatalog.AllPlanets));
    }

    private static async Task GenerateTsA()
    {
        var start = new DateTime(2025, 1, 1);
        var stop = new DateTime(2026, 1, 1);

        var eventRunner = new HelioEventGenerationRunner();
        var events = await eventRunner.GenerateAsync(start, stop, false);

        var rawRunner = new HelioQuadrantL0RawExportRunner();
        await rawRunner.RunAsync(events);

        var jsonRunner = new HelioQuadrantL0ExportRunner();
        await jsonRunner.RunAsync(events);

        Console.WriteLine("TS-A complete.");
    }

    private static async Task GenerateTsB()
    {
        var start = new DateTime(2025, 1, 1);
        var stop = new DateTime(2026, 1, 1);

        var eventRunner = new GeoEventGenerationRunner();
        var events = await eventRunner.GenerateAsync(start, stop, false);

        var rawRunner = new GeoNodeL0RawExportRunner();
        await rawRunner.RunAsync(events);

        var jsonRunner = new GeoNodeL0ExportRunner();
        await jsonRunner.RunAsync(events);

        Console.WriteLine("TS-B complete.");
    }

    // ============================================================
    // TS-C FULL PIPELINE
    // ============================================================

    private static async Task GenerateTsC()
    {
        var eventRunner = new GeoDecNodeEventRunner();
        var events = await eventRunner.GenerateAsync(fastMode: false);

        var rawRunner = new GeoDecNodeL0RawExportRunner();
        await rawRunner.RunAsync(events);

        var jsonRunner = new GeoDecNodeL0ExportRunner();
        await jsonRunner.RunAsync(events);

        Console.WriteLine("TS-C complete.");
    }

    // ============================================================
    // TS-C QUICKTEST (optional filter)
    // ============================================================

    private static async Task QuickTestTsC(string? planetFilter)
    {
        Console.WriteLine("TS-C QUICKTEST (GeoDec Nodes)");
        Console.WriteLine("--------------------------------");

        var runner = new GeoDecNodeEventRunner();
        var events = await runner.GenerateAsync(fastMode: false);

        if (!string.IsNullOrWhiteSpace(planetFilter))
            events = events.Where(e => e.Planet.Equals(planetFilter, StringComparison.OrdinalIgnoreCase));

        var list = events.ToList();

        foreach (var e in list)
            Console.WriteLine($"{e.Planet} {e.EventName} -> JD {e.JulianDate:F9}");

        Console.WriteLine();
        Console.WriteLine($"TS-C QuickTest complete. Events: {list.Count}");
    }

    private static async Task GenerateTsD(string? selection)
    {
        var planets = PlanetCatalog.ParseSelection(selection);

        var runner = new MeshDataGenerationRunner();
        var config = new MeshDataGenerationRunner.TsDMeshRunConfig
        {
            Planets = planets,
            OverwriteExistingFiles = true
        };

        await runner.RunAllPlanetsAsync(config);

        Console.WriteLine("TS-D complete.");
    }

    private static async Task GenerateTsDEarthQuick()
    {
        var runner = new MeshDataGenerationRunner();
        var config = MeshDataGenerationRunner.TsDMeshRunConfig.OnlyEarth();

        await runner.RunAllPlanetsAsync(config);

        Console.WriteLine("TS-D Earth QuickTest complete.");
    }
}