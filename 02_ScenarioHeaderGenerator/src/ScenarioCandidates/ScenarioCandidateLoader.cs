// ============================================================
// FILE: ScenarioCandidateLoader.cs
// STATUS: UPDATE
// ============================================================

using ScenarioHeaderGenerator.ScenarioCandidates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace AstronoData.ScenarioCandidates
{
    public sealed class ScenarioCandidateLoader
    {
        private readonly JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public List<ScenarioCandidate> LoadAll()
        {
            var folder = ScenarioHeaderGenerator.AstronoSpherePaths.GetCandidateReleasedFolder();

            if (!Directory.Exists(folder))
                throw new DirectoryNotFoundException($"Folder not found: {folder}");

            var files = Directory.GetFiles(folder, "SCN_*.json");

            var result = new List<ScenarioCandidate>();

            foreach (var file in files)
            {
                try
                {
                    var json = File.ReadAllText(file);

                    var candidate = JsonSerializer.Deserialize<ScenarioCandidate>(json, _options);

                    if (candidate == null)
                        throw new InvalidDataException("Deserialization returned null.");

                    result.Add(candidate);
                }
                catch (Exception ex)
                {
                    throw new Exception(
                        $"Error in file: {file}{Environment.NewLine}{ex.Message}",
                        ex);
                }
            }

            return result;
        }
    }
}