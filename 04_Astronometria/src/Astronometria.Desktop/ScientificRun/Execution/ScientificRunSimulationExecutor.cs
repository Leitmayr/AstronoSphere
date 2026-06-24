using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Astronometria.Core.Bodies;
using Astronometria.Core.ScientificRun.Build;
using Astronometria.Core.ScientificRun.Models;
using Astronometria.Core.ScientificRun.StateTree;
using Astronometria.Ephemerides.Planetary;
using Astronometria.Ephemerides.VSOP;
using Astronometria.Time.Astro;

namespace Astronometria.ScientificRun.Execution
{
    public static class ScientificRunSimulationExecutor
    {
        public static ScientificSimulationData Execute(
            string repositoryRoot,
            BuildInfo buildInfo,
            ExperimentInputModel experiment,
            GroundTruthInputModel groundTruth,
            TerminalNodeDescriptor terminalNode)
        {
            var planetId = Enum.Parse<PlanetId>(terminalNode.TargetName);

            var terminalPhysicsNodeType = PhysicsStateNodeType.FromValue(
                terminalNode.PhysicsNodeType);

            var stateTreePath = PhysicsStateTreeRegistry.ResolvePath(
                terminalPhysicsNodeType);

            PrintPhysicsStateTreeTrace(
                terminalPhysicsNodeType,
                stateTreePath);

            var vsopDataPath = Path.Combine(
                repositoryRoot,
                "04_Astronometria",
                "src",
                "Astronometria.Ephemerides",
                "VSOP",
                "Data",
                "87A");

            var repository = new VsopRepository(vsopDataPath);
            var provider = new VsopProvider(repository);
            var positionService = new PlanetPositionService(provider);

            var samples = new List<ScientificDataSample>();

            foreach (var gtSample in groundTruth.Data)
            {
                var time = new TTInstant(gtSample.JD);

                var result = PhysicsStateTreeExecutor.Execute(
                    stateTreePath,
                    planetId,
                    time,
                    provider,
                    positionService);

                samples.Add(new ScientificDataSample
                {
                    JD = gtSample.JD,
                    Position = new ScientificVector
                    {
                        X = result.Position.X,
                        Y = result.Position.Y,
                        Z = result.Position.Z
                    },
                    Velocity = new ScientificVector
                    {
                        X = 0.0,
                        Y = 0.0,
                        Z = 0.0
                    }
                });
            }

            return BuildSimulationData(
                buildInfo,
                experiment,
                groundTruth,
                terminalNode,
                terminalPhysicsNodeType,
                samples);
        }

        private static void PrintPhysicsStateTreeTrace(
            PhysicsStateNodeType terminalPhysicsNodeType,
            PhysicsStateTreePath stateTreePath)
        {
            Console.WriteLine("=== PhysicsStateTree ===");
            Console.WriteLine($"TerminalNodeType : {terminalPhysicsNodeType.Value}");
            Console.WriteLine($"Path             : {string.Join(" -> ", stateTreePath.Nodes.Select(node => node.Value))}");
            Console.WriteLine("Executor         : PhysicsStateTreeExecutor");
            Console.WriteLine();
        }

        private static ScientificSimulationData BuildSimulationData(
            BuildInfo buildInfo,
            ExperimentInputModel experiment,
            GroundTruthInputModel groundTruth,
            TerminalNodeDescriptor terminalNode,
            PhysicsStateNodeType terminalPhysicsNodeType,
            List<ScientificDataSample> samples)
        {
            return new ScientificSimulationData
            {
                RunClassification = new ScientificRunClassification(),

                ExperimentRef = new ScientificExperimentRef
                {
                    CatalogNumber = experiment.CatalogNumber,
                    ExperimentID = experiment.ExperimentID,
                    CoreHash = experiment.CoreHash,
                    SourceFile = experiment.SourceFile
                },

                Measurement = new ScientificMeasurement
                {
                    Domain = "Ephemeris",
                    Instrument = "VEC",
                    CorrectionLevel = terminalNode.CorrectionLevel,
                    TimeScale = experiment.Core.Time.TimeScale
                },

                GroundTruthRef = new ScientificGroundTruthRef
                {
                    Provider = "Horizons",
                    DatasetID = groundTruth.DatasetHeader.DatasetID,
                    RequestHash = groundTruth.DatasetHeader.TruthMetadata.RequestHash,
                    SourceFile = groundTruth.SourceFile
                },

                Engine = new ScientificEngine
                {
                    Name = "Astronometria",
                    SimulationModel = new ScientificSimulationModel
                    {
                        Family = "VSOP",
                        Type = "VSOP87A",
                        Truncation = "none",
                        TimeDomain = "TT"
                    },
                    Build = new ScientificBuild
                    {
                        GitCommit = buildInfo.GitCommit,
                        GitBranch = buildInfo.GitBranch
                    }
                },

                EngineCitation = new ScientificEngineCitation
                {
                    Provider = "AstronoSphere.Astronometria",
                    Source = "https://github.com/Leitmayr/AstronoSphere",
                    Citation = "M. Hiemer, Astronometria - An Astronomical Simulation Engine from the AstronoSphere Ephemeris Validation Framework, 2026, https://github.com/Leitmayr/AstronoSphere"
                },

                Provenance = new ScientificProvenance
                {
                    ExperimentFactory = "AstronoSphere.AstronoLab",
                    TruthFactory = "JPL Horizons",
                    SimulationEngine = "AstronoSphere.Astronometria"
                },

                ObservationScene = new ScientificObservationScene
                {
                    SceneContext = new ScientificSceneContext
                    {
                        TemporalMode = "ObservationTime",
                        Time = new ScientificTime
                        {
                            StartJD = experiment.Core.Time.StartJD,
                            StopJD = experiment.Core.Time.StopJD,
                            Step = experiment.Core.Time.Step,
                            TimeScale = experiment.Core.Time.TimeScale
                        },
                        Observer = new ScientificObserver
                        {
                            Type = experiment.Core.Observer.Type,
                            Body = experiment.Core.Observer.Body
                        },
                        Frame = new ScientificFrame
                        {
                            Origin = terminalNode.Origin,
                            Plane = terminalNode.Plane,
                            Epoch = experiment.Core.Frame.Epoch
                        }
                    },
                    TargetSimulations =
                    {
                        new ScientificTargetSimulation
                        {
                            Target = new ScientificTarget
                            {
                                BodyClass = experiment.Core.ObservedObject.BodyClass,
                                Name = terminalNode.TargetName,
                                Abbreviation = terminalNode.TargetAbbreviation
                            },
                            TerminalNode = new ScientificTerminalNode
                            {
                                NodeId = terminalNode.NodeId,
                                NodeType = terminalPhysicsNodeType.Value,
                                NodeRole = terminalNode.NodeRole,
                                Status = "Completed",
                                StateHash = string.Empty,
                                DataHash = string.Empty,
                                Data = samples
                            }
                        }
                    }
                }
            };
        }
    }
}