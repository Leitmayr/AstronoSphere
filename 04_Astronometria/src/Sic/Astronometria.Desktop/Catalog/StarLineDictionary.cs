using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Globalization;
using System.IO;

using System.Linq;

namespace Astronometria
{

    public struct StarLine
    {
        public Point Start; // RA/Dec oder projiziert
        public Point End;

        public StarLine(Point start, Point end)
        {
            Start = start;
            End = end;
        }
    }
    public class StarLineDictionary
    {

        public static Dictionary<string, List<StarLine>> constellationLines
             = new Dictionary<string, List<StarLine>>();
        public static void Load(string filePath)
        {

            var culture = CultureInfo.InvariantCulture;

            foreach (var line in File.ReadLines(filePath).Skip(1))
            {
                var parts = line.Split(',');

                string constellation = parts[0];

                double ra1 = double.Parse(parts[1], culture);
                double dec1 = double.Parse(parts[2], culture);
                double ra2 = double.Parse(parts[3], culture);
                double dec2 = double.Parse(parts[4], culture);

                var p1 = new Point(ra1, dec1);
                var p2 = new Point(ra2, dec2);

                if (!constellationLines.TryGetValue(constellation, out var list))
                {
                    list = new List<StarLine>();
                    constellationLines[constellation] = list;
                }

                list.Add(new StarLine(p1, p2));
            }
        }
    }
}
