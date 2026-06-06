using SpatialAnalysis.Core.Input;
using SpatialAnalysis.Core.Validation;

namespace SpatialAnalysis.Core.Analysis.Models;

public sealed record InvalidRecord(
    int SourceIndex,
    SpatialObjectDto Dto,
    IReadOnlyList<ValidationError> Errors);
