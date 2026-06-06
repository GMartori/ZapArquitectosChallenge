using SpatialAnalysis.Core.Domain.Entities;

namespace SpatialAnalysis.Core.Analysis.Models;

public sealed record ContainmentPair(SpatialObject Inner, SpatialObject Outer);
