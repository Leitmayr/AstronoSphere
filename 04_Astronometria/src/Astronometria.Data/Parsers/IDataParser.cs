using System.Collections.Generic;

namespace Astronometria.Data.Parsers
{
    public interface IDataParser<T>
    {
        IEnumerable<T> Parse(string path);
    }
}