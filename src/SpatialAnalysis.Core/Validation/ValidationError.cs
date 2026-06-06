namespace SpatialAnalysis.Core.Validation;

public sealed record ValidationError(
    ValidationErrorCode Code,
    string Message,
    string? Field = null);
