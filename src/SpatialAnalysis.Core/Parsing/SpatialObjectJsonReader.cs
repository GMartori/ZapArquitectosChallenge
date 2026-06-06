using System.Text.Json;
using SpatialAnalysis.Core.Input;

namespace SpatialAnalysis.Core.Parsing;

public sealed class SpatialObjectJsonReader
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true
    };

    public IReadOnlyList<SpatialObjectDto> ReadFromFile(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("La ruta del archivo no puede estar vacía.", nameof(filePath));
        }

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"No se encontró el archivo de datos: {filePath}", filePath);
        }

        var json = File.ReadAllText(filePath);
        return ReadFromJson(json);
    }

    public IReadOnlyList<SpatialObjectDto> ReadFromJson(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            throw new ArgumentException("El contenido JSON no puede estar vacío.", nameof(json));
        }

        var objects = JsonSerializer.Deserialize<List<SpatialObjectDto>>(json, Options);

        if (objects is null)
        {
            throw new JsonException("No se pudo deserializar la colección de objetos.");
        }

        return objects;
    }
}
