using Astronometria.Core.Coordinates;
using Astronometria.Core.Time;

namespace Astronometria.Projection.Transforms
{
    public interface IAstrometricCorrection
    {
        EquatorialCoord Apply(EquatorialCoord eq, AstroTimeUT time);
    }
}
