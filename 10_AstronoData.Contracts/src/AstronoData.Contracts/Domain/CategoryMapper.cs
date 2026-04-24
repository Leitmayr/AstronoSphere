// ============================================================
// FILE: CategoryMapper.cs
// STATUS: UPDATED (M2.1 Mesh Categories)
// ============================================================

using System;
using System.Text.RegularExpressions;

namespace AstronoData.Contracts.Domain
{
    public static class CategoryMapper
    {
        public static string Normalize(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new Exception("Category is empty.");

            var result = Regex.Replace(input, "([a-z])([A-Z])", "$1 $2");

            return result.Trim().ToUpperInvariant();
        }

        public static string ToAbbreviation(string input)
        {
            var normalized = Normalize(input);

            return normalized switch
            {
                "ASCENDING NODE" => "ANO",
                "APHELION" => "APH",
                "CONJUNCTION" => "CON",
                "DESCENDING NODE" => "DNO",
                "INFERIOR CONJUNCTION" => "INC",

                // M2.1 MESH CATEGORIES
                "MESH SIMULATION CORE" => "MCRE",
                "MESH SIMULATION EXTENDED" => "MXT1",
                "MESH SIMULATION OUTER" => "MXT2",
                "MESH VALIDATION HORIZONS CORE" => "MVH1",
                "MESH VALIDATION HORIZONS EXTENDED" => "MVH2",
                "MESH VALIDATION HORIZONS OUTER" => "MVH3",

                "OPPOSITION" => "OPP",
                "PERIHELION" => "PER",
                "QUADRANT CROSSING" => "QCR",
                "STATION" => "STA",
                "GREATEST WESTERN ELONGATION" => "GWE",
                "GREATEST EASTERN ELONGATION" => "GEE",
                "MISCELLANEOUS DATA POINT" => "MDP",

                // GEO EXTENSIONS
                "GEO EQ ASCENDING NODE" => "GEQ-ANO",
                "GEO EQ DESCENDING NODE" => "GEQ-DNO",

                _ => throw new Exception($"Unknown category: {input} (normalized: {normalized})")
            };
        }
    }
}