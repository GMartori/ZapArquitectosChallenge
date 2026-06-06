using SpatialAnalysis.Core.Input;

namespace SpatialAnalysis.Console.Reporting;

internal static class ObjectListingWriter
{
    public static void WriteSummary(TextWriter output, string dataPath, IReadOnlyList<SpatialObjectDto> objects)
    {
        output.WriteLine("Spatial Analysis — ZAP Arquitectos Challenge");
        output.WriteLine($"Archivo: {dataPath}");
        output.WriteLine($"Total objetos leídos: {objects.Count}");
        output.WriteLine();
        output.WriteLine("--- Objetos (datos crudos) ---");

        foreach (var obj in objects)
        {
            var category = obj.Category is null ? "null" : $"\"{obj.Category}\"";
            output.WriteLine(
                $"Id={obj.Id} | Name={obj.Name} | Category={category} | " +
                $"Origin=({obj.X},{obj.Y},{obj.Z}) | Size=({obj.Width}x{obj.Height}x{obj.Depth})");
        }
    }
}
