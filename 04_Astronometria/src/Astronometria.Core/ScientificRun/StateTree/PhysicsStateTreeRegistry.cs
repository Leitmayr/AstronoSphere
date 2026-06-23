using System;

namespace Astronometria.Core.ScientificRun.StateTree
{
    /// <summary>
    /// PURPOSE:
    /// Defines the frozen internal M2.4.0 physics StateTree paths.
    ///
    /// CONTEXT:
    /// M2.4.0 supports only L0 VEC terminal nodes:
    /// - HELIO-ECL-J2000
    /// - GEO-ECL-J2000
    /// - GEO-EQU-J2000
    ///
    /// CONSTRAINTS:
    /// This registry is intentionally static and explicit.
    /// No dynamic graph selection is allowed in M2.4.0.
    /// </summary>
    public static class PhysicsStateTreeRegistry
    {
        public static PhysicsStateTreePath ResolvePath(PhysicsStateNodeType terminalNodeType)
        {
            if (terminalNodeType == null)
                throw new ArgumentNullException(nameof(terminalNodeType));

            if (terminalNodeType.Equals(PhysicsStateNodeType.HelioEclJ2000VecL0))
            {
                return new PhysicsStateTreePath(new[]
                {
                    PhysicsStateNodeType.HelioEclJ2000VecL0
                });
            }

            if (terminalNodeType.Equals(PhysicsStateNodeType.GeoEclJ2000VecL0))
            {
                return new PhysicsStateTreePath(new[]
                {
                    PhysicsStateNodeType.HelioEclJ2000VecL0,
                    PhysicsStateNodeType.GeoEclJ2000VecL0
                });
            }

            if (terminalNodeType.Equals(PhysicsStateNodeType.GeoEquJ2000VecL0))
            {
                return new PhysicsStateTreePath(new[]
                {
                    PhysicsStateNodeType.HelioEclJ2000VecL0,
                    PhysicsStateNodeType.GeoEclJ2000VecL0,
                    PhysicsStateNodeType.GeoEquJ2000VecL0
                });
            }

            throw new NotSupportedException(
                $"No M2.4 physics StateTree path registered for terminal node '{terminalNodeType}'.");
        }
    }
}