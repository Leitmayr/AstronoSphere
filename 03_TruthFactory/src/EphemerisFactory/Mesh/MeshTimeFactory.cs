using System;
using System.Collections.Generic;
using EphemerisRegression.Domain;

namespace EphemerisRegression.Mesh
{
    public sealed class MeshTimeFactory
    {
        public IReadOnlyList<MeshUtc> Generate(
            EpochDefinition epoch,
            bool isInnerPlanet)
        {
            var step = isInnerPlanet ? epoch.InnerStep : epoch.OuterStep;

            double startJd = JulianDateConverter.ToJulianDay(epoch.StartUtc);
            double stopJd = JulianDateConverter.ToJulianDay(epoch.StopUtc);

            var result = new List<MeshUtc>();

            double current = startJd;

            while (current <= stopJd)
            {
                result.Add(JulianDateConverter.FromJulianDay(current));
                current += step.TotalDays;
            }

            return result;
        }
    }
}