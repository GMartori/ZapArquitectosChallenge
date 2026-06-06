using SpatialAnalysis.Core.Analysis.Models;
using SpatialAnalysis.Core.Domain.Entities;

namespace SpatialAnalysis.Console.Reporting;

internal static class ValidationReportWriter
{
    public static void Write(
        TextWriter output,
        string dataPath,
        ProcessingBatch batch,
        SpatialAnalysisResult? spatialResult = null)
    {
        output.WriteLine("Spatial Analysis — ZAP Arquitectos Challenge");
        output.WriteLine($"Archivo: {dataPath}");
        output.WriteLine();
        output.WriteLine("--- Resumen ---");
        output.WriteLine($"Total objetos procesados: {batch.TotalRead}");
        output.WriteLine($"Objetos válidos: {batch.ValidCount}");
        output.WriteLine($"Objetos inválidos: {batch.InvalidCount}");
        output.WriteLine();

        output.WriteLine("--- Objetos válidos (participan del análisis espacial) ---");
        if (batch.ValidObjects.Count == 0)
        {
            output.WriteLine("(ninguno)");
        }
        else
        {
            foreach (var obj in batch.ValidObjects)
            {
                output.WriteLine(
                    $"Id={obj.Id} | Name={obj.Name} | Category={obj.Category} | " +
                    $"Origin=({obj.Box.Origin.X},{obj.Box.Origin.Y},{obj.Box.Origin.Z}) | " +
                    $"Size=({obj.Box.Dimensions.Width}x{obj.Box.Dimensions.Height}x{obj.Box.Dimensions.Depth})");
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

        if (spatialResult is not null)
        {
            WriteSpatialAnalysis(output, spatialResult);
        }
    }

    private static void WriteSpatialAnalysis(TextWriter output, SpatialAnalysisResult spatialResult)
    {
        output.WriteLine();
        output.WriteLine("--- Análisis espacial (§7.3) ---");
        output.WriteLine($"Intersecciones detectadas: {spatialResult.IntersectionCount}");
        output.WriteLine($"Contenciones detectadas: {spatialResult.ContainmentCount}");
        output.WriteLine();

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
    }

    private static string FormatPair(SpatialObject a, SpatialObject b) =>
        $"{FormatObject(a)} ∩ {FormatObject(b)}";

    private static string FormatObject(SpatialObject obj) =>
        $"{obj.Name} (Id={obj.Id})";
}
