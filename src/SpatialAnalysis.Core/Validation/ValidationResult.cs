namespace SpatialAnalysis.Core.Validation;

public sealed class ValidationResult
{
    public bool IsValid => Errors.Count == 0;
    public IReadOnlyList<ValidationError> Errors { get; }

    private ValidationResult(IReadOnlyList<ValidationError> errors)
    {
        Errors = errors;
    }

    public static ValidationResult Success() => new([]);

    public static ValidationResult Failure(params ValidationError[] errors) =>
        new(errors);

    public static ValidationResult Failure(IEnumerable<ValidationError> errors) =>
        new(errors.ToList());
}
