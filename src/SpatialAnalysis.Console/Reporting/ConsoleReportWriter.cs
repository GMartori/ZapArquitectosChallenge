using System.Text;
using SpatialAnalysis.Core.Analysis.Models;
using SpatialAnalysis.Core.Domain.Entities;

namespace SpatialAnalysis.Console.Reporting;

internal static class ConsoleReportWriter
{
    public static string BuildReport(
        string dataPath,
        ProcessingBatch batch,
        SpatialAnalysisResult spatialResult)
    {
        var buffer = new StringWriter();
        Write(buffer, dataPath, batch, spatialResult);
        return buffer.ToString();
    }

    public static void Write(
        TextWriter output,
        string dataPath,
        ProcessingBatch batch,
        SpatialAnalysisResult spatialResult)
    {
        output.WriteLine("Spatial Analysis — ZAP Arquitectos Challenge");
        output.WriteLine($"Archivo: {dataPath}");
        output.WriteLine($"Generado: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        output.WriteLine();

        WriteSummary(output, batch, spatialResult);
        WriteValidationDetails(output, batch);
        WriteSpatialDetails(output, spatialResult);
    }

    public static void Publish(
        string dataPath,
        ProcessingBatch batch,
        SpatialAnalysisResult spatialResult,
        string? exportPath)
    {
        var report = BuildReport(dataPath, batch, spatialResult);
        global::System.Console.Write(report);

        if (string.IsNullOrWhiteSpace(exportPath))
        {
            return;
        }

        var directory = Path.GetDirectoryName(exportPath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(exportPath, report, Encoding.UTF8);
        global::System.Console.WriteLine();
        global::System.Console.WriteLine($"Reporte exportado: {exportPath}");
    }

    private static void WriteSummary(
        TextWriter output,
        ProcessingBatch batch,
        SpatialAnalysisResult spatialResult)
    {
        output.WriteLine("--- Resumen (§7.5) ---");
        output.WriteLine($"Total objetos procesados: {batch.TotalRead}");
        output.WriteLine($"Objetos válidos: {batch.ValidCount}");
        output.WriteLine($"Objetos inválidos: {batch.InvalidCount}");
        output.WriteLine($"Intersecciones detectadas: {spatialResult.IntersectionCount}");
        output.WriteLine($"Objetos contenidos en otros: {spatialResult.ContainedObjectCount}");
        output.WriteLine($"Objetos aislados: {spatialResult.IsolatedCount}");
        output.WriteLine();
    }

    private static void WriteValidationDetails(TextWriter output, ProcessingBatch batch)
    {
        output.WriteLine("--- Objetos válidos ---");
        if (batch.ValidObjects.Count == 0)
        {
            output.WriteLine("(ninguno)");
        }
        else
        {
            foreach (var obj in batch.ValidObjects)
            {
                output.WriteLine(FormatObjectDetails(obj));
            }
        }

        output.WriteLine();
        output.WriteLine("--- Errores de validación ---");
        if (batch.InvalidRecords.Count == 0)
        {
            output.WriteLine("(ninguno)");
        }
        else
        {
            foreach (var invalid in batch.InvalidRecords)
            {
                output.WriteLine(
                    $"[{invalid.SourceIndex}] Id={invalid.Dto.Id} | Name={invalid.Dto.Name}");
                foreach (var error in invalid.Errors)
                {
                    var field = error.Field is null ? "" : $" ({error.Field})";
                    output.WriteLine($"  - [{error.Code}]{field}: {error.Message}");
                }
            }
        }

        output.WriteLine();
    }

    private static void WriteSpatialDetails(TextWriter output, SpatialAnalysisResult spatialResult)
    {
        output.WriteLine("--- Detalle análisis espacial (§7.3) ---");
        output.WriteLine("Pares en intersección:");
        if (spatialResult.Intersections.Count == 0)
        {
            output.WriteLine("(ninguno)");
        }
        else
        {
            foreach (var pair in spatialResult.Intersections)
            {
                output.WriteLine(FormatPair(pair.First, pair.Second));
            }
        }

        output.WriteLine();
        output.WriteLine("Pares en contención (inner ⊂ outer):");
        if (spatialResult.Containments.Count == 0)
        {
            output.WriteLine("(ninguno)");
        }
        else
        {
            foreach (var pair in spatialResult.Containments)
            {
                output.WriteLine($"{FormatObject(pair.Inner)} ⊂ {FormatObject(pair.Outer)}");
            }
        }

        output.WriteLine();
        output.WriteLine("Objetos aislados:");
        if (spatialResult.IsolatedObjects.Count == 0)
        {
            output.WriteLine("(ninguno)");
        }
        else
        {
            foreach (var obj in spatialResult.IsolatedObjects)
            {
                output.WriteLine(FormatObject(obj));
            }
        }
    }

    private static string FormatObjectDetails(SpatialObject obj) =>
        $"Id={obj.Id} | Name={obj.Name} | Category={obj.Category} | " +
        $"Origin=({obj.Box.Origin.X},{obj.Box.Origin.Y},{obj.Box.Origin.Z}) | " +
        $"Size=({obj.Box.Dimensions.Width}x{obj.Box.Dimensions.Height}x{obj.Box.Dimensions.Depth})";

    private static string FormatPair(SpatialObject a, SpatialObject b) =>
        $"{FormatObject(a)} ∩ {FormatObject(b)}";

    private static string FormatObject(SpatialObject obj) =>
        $"{obj.Name} (Id={obj.Id})";
}
