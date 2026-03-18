using System.Collections.Generic;

namespace AstroSim.Data.Parsers
{
    public interface IDataParser<T>
    {
        IEnumerable<T> Parse(string path);
    }
}