using System;

namespace Astronometria.ScientificRun.Cli
{
    public static class CliParser
    {
        public static CliArguments Parse(string[] args)
        {
            if (args.Length == 0)
                return new CliArguments();

            if (args.Length == 1 &&
                (args[0].Equals("--help", StringComparison.OrdinalIgnoreCase) ||
                 args[0].Equals("-h", StringComparison.OrdinalIgnoreCase)))
            {
                return new CliArguments
                {
                    ShowHelp = true
                };
            }

            if (args.Length == 2 &&
                args[0].Equals("--catalog", StringComparison.OrdinalIgnoreCase))
            {
                var catalogNumber = args[1].Trim();

                if (string.IsNullOrWhiteSpace(catalogNumber))
                    throw new ArgumentException("Catalog number must not be empty.");

                return new CliArguments
                {
                    CatalogNumber = catalogNumber
                };
            }

            throw new ArgumentException(
                "Unsupported arguments. Use: Astronometria.Desktop.exe --catalog AS-000003");
        }
    }
}