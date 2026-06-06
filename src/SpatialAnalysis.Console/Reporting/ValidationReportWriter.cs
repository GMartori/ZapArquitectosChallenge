using SpatialAnalysis.Core.Analysis.Models;

namespace SpatialAnalysis.Console.Reporting;

internal static class ValidationReportWriter
{
    public static void Write(TextWriter output, string dataPath, ProcessingBatch batch)
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
    }
}
