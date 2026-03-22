using System;
using System.Globalization;
using System.IO;
using System.Linq;
using AstroSim.Data.Parsers.BrightStarCatalog;
using NUnit.Framework;

namespace AstroSim.Data.Tests.DataParsing
{
    [TestFixture]
    public class BrightStarCatalogParserTests
    {
        [Test]
        public void Parse_ShouldRead_HR_RA_DEC_Mag_SpecType_FromKnownLine()
        {
            // Arrange
            var tmp = Path.Combine(Path.GetTempPath(), $"bsc_test_{Guid.NewGuid():N}.txt");

            // Wir bauen eine Zeile, die zu deinem Parser passt:
            // - Startet mit "{"
            // - CSV-like Felder mit Indizes:
            //   f[1]=HR, f[6]=Mag, f[9]=SpecType, f[21]=RAdeg, f[23]=DECdeg
            //
            // Wir erzeugen bewusst 24 Felder (Index 0..23).
            var f = new string[24];

            // Default füllen (damit Split(',',) 24 Felder ergibt)
            for (int i = 0; i < f.Length; i++) f[i] = "\"\"";

            f[0] = "\"0\"";
            f[1] = "\"3\"";         // HR
            f[2] = "\"Alpha\"";     // Name
            f[3] = "\"HD123\"";     // HD
            f[6] = "1.23";          // Mag (InvariantCulture)
            f[9] = "\"A0\"";        // SpecTypeShort
            f[10] = "\"And\"";       // ConstellationShort
            f[11] = "\"Andromeda\"";
            f[12] = "\"Andromeda\"";
            f[14] = "\"α\"";
            f[21] = "1.33375";       // RAdeg
            f[23] = "-5.7075";       // DECdeg

            var line = "{ " + string.Join(",", f) + " },";
            File.WriteAllLines(tmp, new[] { line });

            try
            {
                var parser = new BrightStarCatalogParser();

                // Act
                var rec = parser.Parse(tmp).Single();

                // Assert
                Assert.That(rec.HarvardRevisedNumber, Is.EqualTo(3));
                Assert.That(rec.RightAscensionDeg, Is.EqualTo(1.33375).Within(1e-12));
                Assert.That(rec.DeclinationDeg, Is.EqualTo(-5.7075).Within(1e-12));

                Assert.That(rec.VisualMagnitude, Is.EqualTo(1.23).Within(1e-12));
                Assert.That(rec.SpectralTypeShort, Is.EqualTo("A0"));
            }
            finally
            {
                // Cleanup
                if (File.Exists(tmp)) File.Delete(tmp);
            }
        }

        [Test]
        public void Parse_ShouldSkip_NonDataLines_And_InvalidLines()
        {
            // Arrange
            var tmp = Path.Combine(Path.GetTempPath(), $"bsc_test_{Guid.NewGuid():N}.txt");

            // Gültige Zeile bauen (24 Felder: Index 0..23)
            var f = new string[24];
            for (int i = 0; i < f.Length; i++) f[i] = "\"\"";

            f[1] = "\"3\"";       // HR
            f[6] = "1.23";        // Mag
            f[9] = "\"A0\"";      // SpecTypeShort
            f[21] = "1.33375";     // RAdeg
            f[23] = "-5.7075";     // DECdeg

            var valid = "{ " + string.Join(",", f) + " },";

            File.WriteAllLines(tmp, new[]
            {
            "",
            "   // comment",
            "not a data line",
            "{ \"x\",\"y\" },",   // zu wenige Felder -> invalid
            valid                // genau 1 gültige Datenzeile
            });

            try
            {
                var parser = new BrightStarCatalogParser();

                // Act
                var list = parser.Parse(tmp).ToList();

                // Assert
                Assert.That(list.Count, Is.EqualTo(1));
                Assert.That(list[0].HarvardRevisedNumber, Is.EqualTo(3));
            }
            finally
            {
                if (File.Exists(tmp)) File.Delete(tmp);
            }
        }
    }
}