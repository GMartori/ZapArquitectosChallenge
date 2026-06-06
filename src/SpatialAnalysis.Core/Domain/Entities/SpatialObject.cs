using SpatialAnalysis.Core.Domain.ValueObjects;

namespace SpatialAnalysis.Core.Domain.Entities;

/// <summary>
/// Objeto espacial válido. Solo se crea tras pasar la validación.
/// </summary>
public sealed record SpatialObject(
    int Id,
    string Name,
    string Category,
    AxisAlignedBox Box);
