using Astronometria.Core.Bodies;
using Astronometria.Core.ScientificRun.Models;
using Astronometria.Core.ScientificRun.StateTree;
using Astronometria.Ephemerides.Planetary;
using Astronometria.Ephemerides.VSOP;
using Astronometria.Time.Astro;
using System;

namespace Astronometria.ScientificRun.Execution
{
    /// <summary>
    /// PURPOSE:
    /// Executes the internal M2.4.0 PHYS StateTree path for one target and one time instant.
    ///
    /// CONTEXT:
    /// This executor is the bridge between the canonical StateTree identity and the
    /// existing VSOP87A based Astronometria engine implementation.
    ///
    /// CONSTRAINTS:
    /// M2.4.0 supports only L0 vector nodes.
    /// Persisted JSON may still keep legacy VSOP87.* node names.
    /// </summary>
    public static class PhysicsStateTreeExecutor
    {
        private const double J2000MeanObliquityDegrees = 23.439291111111111;

        public static PhysicsStateTreeExecutionResult Execute(
            PhysicsStateTreePath path,
            PlanetId planetId,
            TTInstant time,
            VsopProvider provider,
            PlanetPositionService positionService)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            if (positionService == null)
                throw new ArgumentNullException(nameof(positionService));

            ScientificVector? current = null;

            foreach (var node in path.Nodes)
            {
                if (node.Equals(PhysicsStateNodeType.HelioEclJ2000VecL0))
                {
                    var state = provider.GetHeliocentricState(planetId, time);

                    current = new ScientificVector
                    {
                        X = state.Position.X,
                        Y = state.Position.Y,
                        Z = state.Position.Z
                    };

                    continue;
                }

                if (node.Equals(PhysicsStateNodeType.GeoEclJ2000VecL0))
                {
                    var state = positionService.GetGeocentricEclipticState(planetId, time);

                    current = new ScientificVector
                    {
                        X = state.Position.X,
                        Y = state.Position.Y,
                        Z = state.Position.Z
                    };

                    continue;
                }

                if (node.Equals(PhysicsStateNodeType.GeoEquJ2000VecL0))
                {
                    if (current == null)
                        throw new InvalidOperationException(
                            "GEO-EQU node requires an existing GEO-ECL vector.");

                    current = RotateEclipticToEquatorial(current);
                    continue;
                }

                throw new NotSupportedException(
                    $"Unsupported physics StateTree node during execution: '{node}'.");
            }

            if (current == null)
                throw new InvalidOperationException("Physics StateTree path produced no result.");

            return new PhysicsStateTreeExecutionResult(current);
        }

        private static ScientificVector RotateEclipticToEquatorial(ScientificVector ecliptic)
        {
            var epsilon = J2000MeanObliquityDegrees * Math.PI / 180.0;
            var cosEpsilon = Math.Cos(epsilon);
            var sinEpsilon = Math.Sin(epsilon);

            return new ScientificVector
            {
                X = ecliptic.X,
                Y = ecliptic.Y * cosEpsilon - ecliptic.Z * sinEpsilon,
                Z = ecliptic.Y * sinEpsilon + ecliptic.Z * cosEpsilon
            };
        }
    }

    public sealed class PhysicsStateTreeExecutionResult
    {
        public PhysicsStateTreeExecutionResult(ScientificVector position)
        {
            Position = position ?? throw new ArgumentNullException(nameof(position));
        }

        public ScientificVector Position { get; }
    }
}