namespace SpatialAnalysis.Core.Input;

/// <summary>
/// Representación 1:1 del JSON de entrada. La validación se aplica en una fase posterior.
/// </summary>
public sealed record SpatialObjectDto(
    int Id,
    string Name,
    string? Category,
    double X,
    double Y,
    double Z,
    double Width,
    double Height,
    double Depth);
