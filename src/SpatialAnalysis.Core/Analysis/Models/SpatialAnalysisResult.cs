using SpatialAnalysis.Core.Domain.Entities;

namespace SpatialAnalysis.Core.Analysis.Models;

public sealed class SpatialAnalysisResult
{
    public required IReadOnlyList<ObjectPair> Intersections { get; init; }
    public required IReadOnlyList<ContainmentPair> Containments { get; init; }
    public required IReadOnlyList<SpatialObject> IsolatedObjects { get; init; }

    public int IntersectionCount => Intersections.Count;
    public int ContainmentCount => Containments.Count;
    public int ContainedObjectCount => Containments.Select(c => c.Inner.Id).Distinct().Count();
    public int IsolatedCount => IsolatedObjects.Count;
}
