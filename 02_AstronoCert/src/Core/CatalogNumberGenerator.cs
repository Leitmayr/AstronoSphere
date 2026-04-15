// ============================================================
// FILE: CatalogNumberGenerator.cs
// STATUS: DEBUG BUILD
// PURPOSE:
// - show releasedFolder
// - show every file considered
// - show every parsing step
// - fail with precise context
// ============================================================

using System;
using System.IO;
using System.Linq;

namespace AstronoCert
{
    public static class CatalogNumberGenerator
    {
        public static int GetNextStart(string releasedFolder)
        {
            Console.WriteLine("=== CatalogNumberGenerator DEBUG ===");
            Console.WriteLine($"releasedFolder = {releasedFolder}");
            Console.WriteLine($"Directory.Exists = {Directory.Exists(releasedFolder)}");

            if (!Directory.Exists(releasedFolder))
            {
                Console.WriteLine("releasedFolder does not exist -> returning 1");
                Console.WriteLine("====================================");
                return 1;
            }

            var files = Directory.GetFiles(releasedFolder, "*", SearchOption.TopDirectoryOnly);

            Console.WriteLine($"File count = {files.Length}");

            if (!files.Any())
            {
                Console.WriteLine("No files found -> returning 1");
                Console.WriteLine("====================================");
                return 1;
            }

            var numbers = files
                .Select(ParseCatalogNumberFromPath)
                .ToList();

            Console.WriteLine("Parsed catalog numbers:");
            foreach (var n in numbers)
            {
                Console.WriteLine($"  {n}");
            }

            var max = numbers.Max();

            Console.WriteLine($"Max catalog number = {max}");
            Console.WriteLine($"Next catalog number = {max + 1}");
            Console.WriteLine("====================================");

            return max + 1;
        }

        private static int ParseCatalogNumberFromPath(string path)
        {
            var fileName = Path.GetFileName(path);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);

            Console.WriteLine();
            Console.WriteLine("--- Parse candidate ---");
            Console.WriteLine($"path                     = {path}");
            Console.WriteLine($"fileName                 = {fileName}");
            Console.WriteLine($"fileNameWithoutExtension = {fileNameWithoutExtension}");

            // Erwartung aktuell:
            // AS-000001__PLANET-...__HELIO-...
            var firstBlock = fileNameWithoutExtension.Split(new[] { "__" }, StringSplitOptions.None)[0];

            Console.WriteLine($"firstBlock               = {firstBlock}");

            var numericPart = firstBlock.Replace("AS-", "");

            Console.WriteLine($"numericPart              = {numericPart}");

            try
            {
                var number = int.Parse(numericPart);
                Console.WriteLine($"parsed number            = {number}");
                return number;
            }
            catch (Exception ex)
            {
                Console.WriteLine("!!! PARSE ERROR !!!");
                Console.WriteLine($"Exception type           = {ex.GetType().FullName}");
                Console.WriteLine($"Exception message        = {ex.Message}");
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!");

                throw new Exception(
                    "CatalogNumberGenerator failed while parsing catalog number." + Environment.NewLine +
                    $"path={path}" + Environment.NewLine +
                    $"fileName={fileName}" + Environment.NewLine +
                    $"fileNameWithoutExtension={fileNameWithoutExtension}" + Environment.NewLine +
                    $"firstBlock={firstBlock}" + Environment.NewLine +
                    $"numericPart={numericPart}",
                    ex);
            }
        }
    }
}