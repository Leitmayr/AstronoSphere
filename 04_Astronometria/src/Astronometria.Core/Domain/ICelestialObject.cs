using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astronometria.Core.Domain
{

    public interface ICelestialObject
    {
        string Id { get; }          // stabile ID: "Jupiter", "HR7001" ...
        string DisplayName { get; } // UI-Name
    }
}
