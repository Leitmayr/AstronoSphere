using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Astronometria.Core.ScientificRun.Models;

namespace Astronometria.Core.ScientificRun.IO
{
    public static class ScientificSimulationJsonWriter
    {
        public static void Write(string path, ScientificSimulationData data)
        {
            var json = BuildJson(data);
            File.WriteAllText(path, json, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
        }

        private static string BuildJson(ScientificSimulationData data)
        {
            var sb = new StringBuilder();

            sb.AppendLine("{");
            AppendRunClassification(sb, data);
            AppendExperimentRef(sb, data);
            AppendMeasurement(sb, data);
            AppendGroundTruthRef(sb, data);
            AppendEngine(sb, data);
            AppendEngineCitation(sb, data);
            AppendProvenance(sb, data);
            AppendObservationScene(sb, data);
            sb.AppendLine("}");

            return sb.ToString();
        }

        private static void AppendRunClassification(StringBuilder sb, ScientificSimulationData data)
        {
            sb.AppendLine("  \"RunClassification\": {");
            JsonProp(sb, 4, "RunType", data.RunClassification.RunType, comma: true);
            JsonProp(sb, 4, "InputType", data.RunClassification.InputType, comma: true);
            JsonProp(sb, 4, "TargetCardinality", data.RunClassification.TargetCardinality, comma: false);
            sb.AppendLine("  },");
        }

        private static void AppendExperimentRef(StringBuilder sb, ScientificSimulationData data)
        {
            sb.AppendLine("  \"ExperimentRef\": {");
            JsonProp(sb, 4, "CatalogNumber", data.ExperimentRef.CatalogNumber, comma: true);
            JsonProp(sb, 4, "ExperimentID", data.ExperimentRef.ExperimentID, comma: true);
            JsonProp(sb, 4, "CoreHash", data.ExperimentRef.CoreHash, comma: true);
            JsonProp(sb, 4, "SourceFile", data.ExperimentRef.SourceFile, comma: false);
            sb.AppendLine("  },");
        }

        private static void AppendMeasurement(StringBuilder sb, ScientificSimulationData data)
        {
            sb.AppendLine("  \"Measurement\": {");
            JsonProp(sb, 4, "Domain", data.Measurement.Domain, comma: true);
            JsonProp(sb, 4, "Instrument", data.Measurement.Instrument, comma: true);
            JsonProp(sb, 4, "CorrectionLevel", data.Measurement.CorrectionLevel, comma: true);
            JsonProp(sb, 4, "TimeScale", data.Measurement.TimeScale, comma: false);
            sb.AppendLine("  },");
        }

        private static void AppendGroundTruthRef(StringBuilder sb, ScientificSimulationData data)
        {
            sb.AppendLine("  \"GroundTruthRef\": {");
            JsonProp(sb, 4, "Provider", data.GroundTruthRef.Provider, comma: true);
            JsonProp(sb, 4, "DatasetID", data.GroundTruthRef.DatasetID, comma: true);
            JsonProp(sb, 4, "RequestHash", data.GroundTruthRef.RequestHash, comma: true);
            JsonProp(sb, 4, "SourceFile", data.GroundTruthRef.SourceFile, comma: false);
            sb.AppendLine("  },");
        }

        private static void AppendEngine(StringBuilder sb, ScientificSimulationData data)
        {
            sb.AppendLine("  \"Engine\": {");
            JsonProp(sb, 4, "Name", data.Engine.Name, comma: true);
            sb.AppendLine("    \"SimulationModel\": {");
            JsonProp(sb, 6, "Family", data.Engine.SimulationModel.Family, comma: true);
            JsonProp(sb, 6, "Type", data.Engine.SimulationModel.Type, comma: true);
            JsonProp(sb, 6, "Truncation", data.Engine.SimulationModel.Truncation, comma: true);
            JsonProp(sb, 6, "TimeDomain", data.Engine.SimulationModel.TimeDomain, comma: false);
            sb.AppendLine("    },");
            sb.AppendLine("    \"Build\": {");
            JsonProp(sb, 6, "GitCommit", data.Engine.Build.GitCommit, comma: true);
            JsonProp(sb, 6, "GitBranch", data.Engine.Build.GitBranch, comma: false);
            sb.AppendLine("    }");
            sb.AppendLine("  },");
        }

        private static void AppendEngineCitation(StringBuilder sb, ScientificSimulationData data)
        {
            sb.AppendLine("  \"EngineCitation\": {");
            JsonProp(sb, 4, "Provider", data.EngineCitation.Provider, comma: true);
            JsonProp(sb, 4, "Source", data.EngineCitation.Source, comma: true);
            JsonProp(sb, 4, "Citation", data.EngineCitation.Citation, comma: false);
            sb.AppendLine("  },");
        }

        private static void AppendProvenance(StringBuilder sb, ScientificSimulationData data)
        {
            sb.AppendLine("  \"Provenance\": {");
            JsonProp(sb, 4, "ExperimentFactory", data.Provenance.ExperimentFactory, comma: true);
            JsonProp(sb, 4, "TruthFactory", data.Provenance.TruthFactory, comma: true);
            JsonProp(sb, 4, "SimulationEngine", data.Provenance.SimulationEngine, comma: false);
            sb.AppendLine("  },");
        }

        private static void AppendObservationScene(StringBuilder sb, ScientificSimulationData data)
        {
            var scene = data.ObservationScene;
            var targetSimulation = scene.TargetSimulations[0];

            sb.AppendLine("  \"ObservationScene\": {");
            sb.AppendLine("    \"SceneContext\": {");
            JsonProp(sb, 6, "TemporalMode", scene.SceneContext.TemporalMode, comma: true);

            sb.AppendLine("      \"Time\": {");
            JsonNumberProp(sb, 8, "StartJD", scene.SceneContext.Time.StartJD, comma: true);
            JsonNumberProp(sb, 8, "StopJD", scene.SceneContext.Time.StopJD, comma: true);
            JsonProp(sb, 8, "Step", scene.SceneContext.Time.Step, comma: true);
            JsonProp(sb, 8, "TimeScale", scene.SceneContext.Time.TimeScale, comma: false);
            sb.AppendLine("      },");

            sb.AppendLine("      \"Observer\": {");
            JsonProp(sb, 8, "Type", scene.SceneContext.Observer.Type, comma: true);
            JsonProp(sb, 8, "Body", scene.SceneContext.Observer.Body, comma: false);
            sb.AppendLine("      },");

            sb.AppendLine("      \"Frame\": {");
            JsonProp(sb, 8, "Origin", scene.SceneContext.Frame.Origin, comma: true);
            JsonProp(sb, 8, "Plane", scene.SceneContext.Frame.Plane, comma: true);
            JsonProp(sb, 8, "Epoch", scene.SceneContext.Frame.Epoch, comma: false);
            sb.AppendLine("      }");
            sb.AppendLine("    },");

            sb.AppendLine("    \"TargetSimulations\": [");
            sb.AppendLine("      {");
            sb.AppendLine("        \"Target\": {");
            JsonProp(sb, 10, "BodyClass", targetSimulation.Target.BodyClass, comma: true);
            JsonProp(sb, 10, "Name", targetSimulation.Target.Name, comma: true);
            JsonProp(sb, 10, "Abbreviation", targetSimulation.Target.Abbreviation, comma: false);
            sb.AppendLine("        },");

            sb.AppendLine("        \"TerminalNode\": {");
            JsonProp(sb, 10, "NodeId", targetSimulation.TerminalNode.NodeId, comma: true);
            JsonProp(sb, 10, "NodeType", targetSimulation.TerminalNode.NodeType, comma: true);
            JsonProp(sb, 10, "NodeRole", targetSimulation.TerminalNode.NodeRole, comma: true);
            JsonProp(sb, 10, "Status", targetSimulation.TerminalNode.Status, comma: true);
            JsonProp(sb, 10, "StateHash", targetSimulation.TerminalNode.StateHash, comma: true);
            JsonProp(sb, 10, "DataHash", targetSimulation.TerminalNode.DataHash, comma: true);
            sb.AppendLine("          \"Data\": [");

            for (var i = 0; i < targetSimulation.TerminalNode.Data.Count; i++)
            {
                var sample = targetSimulation.TerminalNode.Data[i];
                var comma = i < targetSimulation.TerminalNode.Data.Count - 1 ? "," : string.Empty;

                sb.AppendLine("            {");
                sb.AppendLine($"              \"JD\": {FormatNumber(sample.JD)},");
                sb.AppendLine($"              \"Position\": {{\"X\":{FormatNumber(sample.Position.X)},\"Y\":{FormatNumber(sample.Position.Y)},\"Z\":{FormatNumber(sample.Position.Z)}}},");
                sb.AppendLine($"              \"Velocity\": {{\"X\":{FormatNumber(sample.Velocity.X)},\"Y\":{FormatNumber(sample.Velocity.Y)},\"Z\":{FormatNumber(sample.Velocity.Z)}}}");
                sb.AppendLine($"            }}{comma}");
            }

            sb.AppendLine("          ]");
            sb.AppendLine("        }");
            sb.AppendLine("      }");
            sb.AppendLine("    ]");
            sb.AppendLine("  }");
        }

        private static void JsonProp(StringBuilder sb, int indent, string name, string value, bool comma)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            sb.Append(' ', indent);
            sb.Append("\"");
            sb.Append(name);
            sb.Append("\": ");
            sb.Append(JsonSerializer.Serialize(value, options));
            sb.AppendLine(comma ? "," : string.Empty);
        }

        private static void JsonNumberProp(StringBuilder sb, int indent, string name, double value, bool comma)
        {
            sb.Append(' ', indent);
            sb.Append("\"");
            sb.Append(name);
            sb.Append("\": ");
            sb.Append(FormatNumber(value));
            sb.AppendLine(comma ? "," : string.Empty);
        }

        private static string FormatNumber(double value)
        {
            return value.ToString("G17", CultureInfo.InvariantCulture);
        }
    }
}