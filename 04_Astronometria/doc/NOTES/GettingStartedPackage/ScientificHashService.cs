using System;
using Astronometria.Core.ScientificRun.Models;
using AstronoData.Contracts.Hashing;

namespace Astronometria.Core.ScientificRun.Hashing
{
    public static class ScientificHashService
    {
        public static string ComputeStateHash(
            ScientificSimulationData simulationData)
        {
            if (simulationData == null)
                throw new ArgumentNullException(nameof(simulationData));

            var targetSimulation =
                simulationData
                    .ObservationScene
                    .TargetSimulations[0];

            var model = new StateHashModel
            {
                EngineName = simulationData.Engine.Name,
                SimulationFamily = simulationData.Engine.SimulationModel.Family,
                SimulationType = simulationData.Engine.SimulationModel.Type,
                SimulationTruncation = simulationData.Engine.SimulationModel.Truncation,
                SimulationTimeDomain = simulationData.Engine.SimulationModel.TimeDomain,

                TemporalMode = simulationData.ObservationScene.SceneContext.TemporalMode,

                StartJD = simulationData.ObservationScene.SceneContext.Time.StartJD,
                StopJD = simulationData.ObservationScene.SceneContext.Time.StopJD,
                Step = simulationData.ObservationScene.SceneContext.Time.Step,
                TimeScale = simulationData.ObservationScene.SceneContext.Time.TimeScale,

                ObserverType = simulationData.ObservationScene.SceneContext.Observer.Type,
                ObserverBody = simulationData.ObservationScene.SceneContext.Observer.Body,

                FrameOrigin = simulationData.ObservationScene.SceneContext.Frame.Origin,
                FramePlane = simulationData.ObservationScene.SceneContext.Frame.Plane,
                FrameType = simulationData.ObservationScene.SceneContext.Frame.Type,
                FrameEpoch = simulationData.ObservationScene.SceneContext.Frame.Epoch,
                RefSystem = simulationData.ObservationScene.SceneContext.Frame.RefSystem,

                TargetBodyClass = targetSimulation.Target.BodyClass,
                TargetName = targetSimulation.Target.Name,
                TargetAbbreviation = targetSimulation.Target.Abbreviation,

                NodeId = targetSimulation.TerminalNode.NodeId,
                NodeType = targetSimulation.TerminalNode.NodeType,

                Output = simulationData.Measurement.Instrument,
                CorrectionLevel = simulationData.Measurement.CorrectionLevel
            };

            var canonical = Canonicalizer.Build(model);

            return HashCalculator.Compute(canonical);
        }

        public static string ComputeDataHash(
            ScientificSimulationData simulationData)
        {
            if (simulationData == null)
                throw new ArgumentNullException(nameof(simulationData));

            var terminalNode =
                simulationData
                    .ObservationScene
                    .TargetSimulations[0]
                    .TerminalNode;

            var model = new DataHashModel();

            foreach (var sample in terminalNode.Data)
            {
                model.Samples.Add(new DataHashSample
                {
                    JD = sample.JD,
                    X = sample.Position.X,
                    Y = sample.Position.Y,
                    Z = sample.Position.Z
                });
            }

            var canonical = Canonicalizer.Build(model);

            return HashCalculator.Compute(canonical);
        }
    }
}