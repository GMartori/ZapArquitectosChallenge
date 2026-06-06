using SpatialAnalysis.Core.Domain.Entities;

namespace SpatialAnalysis.Core.Analysis.Models;

public sealed class ProcessingBatch
{
    public required int TotalRead { get; init; }
    public required IReadOnlyList<SpatialObject> ValidObjects { get; init; }
    public required IReadOnlyList<InvalidRecord> InvalidRecords { get; init; }

    public int ValidCount => ValidObjects.Count;
    public int InvalidCount => InvalidRecords.Count;
}
