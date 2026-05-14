using System;
using System.Collections.Generic;
using System.IO;
using Astronometria.Core.Bodies;
using Astronometria.Core.Geometry;
using Astronometria.Core.ScientificRun.Build;
using Astronometria.Core.ScientificRun.Models;
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

                var state = GetEngineState(
                    experiment.Core.Frame.Type,
                    planetId,
                    time,
                    provider,
                    positionService);

                samples.Add(new ScientificDataSample
                {
                    JD = gtSample.JD,
                    Position = new ScientificVector
                    {
                        X = state.Position.X,
                        Y = state.Position.Y,
                        Z = state.Position.Z
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
                samples);
        }

        private static StateVector GetEngineState(
            string frameType,
            PlanetId planetId,
            TTInstant time,
            VsopProvider provider,
            PlanetPositionService positionService)
        {
            return frameType switch
            {
                "HelioEcliptic" => provider.GetHeliocentricState(planetId, time),
                "GeoEcliptic" => positionService.GetGeocentricEclipticState(planetId, time),
                _ => throw new NotSupportedException(
                    $"Unsupported frame type for ScientificRun execution: '{frameType}'.")
            };
        }

        private static ScientificSimulationData BuildSimulationData(
            BuildInfo buildInfo,
            ExperimentInputModel experiment,
            GroundTruthInputModel groundTruth,
            TerminalNodeDescriptor terminalNode,
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
                            Type = experiment.Core.Frame.Type,
                            Epoch = experiment.Core.Frame.Epoch,
                            RefSystem = terminalNode.RefSystem
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
                                NodeType = terminalNode.NodeType,
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