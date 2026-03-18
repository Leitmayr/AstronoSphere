namespace AstroSim.Core.Geometry
{
    /// <summary>
    /// Represents a Cartesian state vector in J2000 reference frame.
    /// Position in AU.
    /// Velocity in AU per Julian day.
    /// </summary>
    public readonly struct StateVector
    {
        public Vector3 Position { get; }
        public Vector3 Velocity { get; }

        public StateVector(Vector3 position, Vector3 velocity)
        {
            Position = position;
            Velocity = velocity;
        }
    }
}
