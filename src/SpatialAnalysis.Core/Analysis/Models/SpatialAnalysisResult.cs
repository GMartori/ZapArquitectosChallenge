namespace SpatialAnalysis.Core.Analysis.Models;

public sealed class SpatialAnalysisResult
{
    public required IReadOnlyList<ObjectPair> Intersections { get; init; }
    public required IReadOnlyList<ContainmentPair> Containments { get; init; }

    public int IntersectionCount => Intersections.Count;
    public int ContainmentCount => Containments.Count;
}
