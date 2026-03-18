using AstroSim.Core.Coordinates;
using AstroSim.Core.Time;

namespace AstroSim.Projection.Transforms
{
    public interface IAstrometricCorrection
    {
        EquatorialCoord Apply(EquatorialCoord eq, AstroTimeUT time);
    }
}
