using System;
using System.Collections.Generic;
using EphemerisRegression.Mesh;
using EphemerisRegression.Domain;

namespace EphemerisRegression.Batching
{
    public sealed class MeshBatchGenerator
    {
        private const int MaxStepsPerRequest = 2000;

        public IReadOnlyList<MeshBatchDefinition> GenerateBatches(
            EpochDefinition epoch,
            string planetName,
            int planetCode,
            bool isInnerPlanet)
        {
            var stepDays = (isInnerPlanet ? epoch.InnerStep : epoch.OuterStep).TotalDays;

            double startJd = JulianDateConverter.ToJulianDay(epoch.StartUtc);
            double stopJd = JulianDateConverter.ToJulianDay(epoch.StopUtc);

            var batches = new List<MeshBatchDefinition>();

            double currentStart = startJd;

            while (currentStart <= stopJd)
            {
                int remainingSteps = (int)Math.Floor((stopJd - currentStart) / stepDays) + 1;
                int stepCount = Math.Min(MaxStepsPerRequest, remainingSteps);

                double currentStop = currentStart + (stepCount - 1) * stepDays;

                if (currentStop > stopJd)
                    currentStop = stopJd;

                batches.Add(new MeshBatchDefinition(
                    epoch.EpochType,
                    planetName,
                    planetCode,
                    JulianDateConverter.FromJulianDay(currentStart),
                    JulianDateConverter.FromJulianDay(currentStop),
                    stepDays,
                    stepCount
                ));

                currentStart = currentStop + stepDays;
            }

            return batches;
        }
    }
}