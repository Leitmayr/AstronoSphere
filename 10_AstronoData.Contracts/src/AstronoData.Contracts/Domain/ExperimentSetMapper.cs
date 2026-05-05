using System;
using System.Globalization;

namespace AstronoData.Contracts.Domain
{
    public static class ExperimentSetMapper
    {
        public static ExperimentSet Map(string catalogNumber)
        {
            var number = ParseCatalogNumber(catalogNumber);

            if (number >= 1 && number <= 12)
                return ExperimentSet.Holy12;

            if (number >= 13 && number <= 145)
            {
                if (number == 20 || number == 34 || number == 48)
                    throw new InvalidOperationException($"Deprecated experiment not mapped: {catalogNumber}");

                if (number >= 73 && number < 145)
                    throw new InvalidOperationException($"Deprecated experiment not mapped: {catalogNumber}");

                return ExperimentSet.Catalog;
            }

            if (number >= 146 && number <= 374)
                return ExperimentSet.Mesh;

            throw new InvalidOperationException(
                $"CatalogNumber is not mapped to an ExperimentSet: {catalogNumber}");
        }

        public static bool IsMapped(string catalogNumber)
        {
            try
            {
                _ = Map(catalogNumber);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static int ParseCatalogNumber(string catalogNumber)
        {
            if (string.IsNullOrWhiteSpace(catalogNumber))
                throw new ArgumentException("CatalogNumber must not be empty.", nameof(catalogNumber));

            if (!catalogNumber.StartsWith("AS-", StringComparison.Ordinal))
                throw new FormatException($"Invalid CatalogNumber format: {catalogNumber}");

            var numberPart = catalogNumber.Substring(3);

            if (!int.TryParse(numberPart, NumberStyles.None, CultureInfo.InvariantCulture, out var number))
                throw new FormatException($"Invalid CatalogNumber number part: {catalogNumber}");

            return number;
        }
    }
}