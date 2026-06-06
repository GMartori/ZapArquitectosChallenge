using SpatialAnalysis.Core.Input;

namespace SpatialAnalysis.Core.Validation.Rules;

public sealed class PositiveDimensionsRule : IObjectValidationRule
{
    public void Validate(SpatialObjectDto dto, ICollection<ValidationError> errors)
    {
        ValidateDimension(dto.Width, nameof(SpatialObjectDto.Width), errors);
        ValidateDimension(dto.Height, nameof(SpatialObjectDto.Height), errors);
        ValidateDimension(dto.Depth, nameof(SpatialObjectDto.Depth), errors);
    }

    private static void ValidateDimension(double value, string field, ICollection<ValidationError> errors)
    {
        if (value <= 0)
        {
            errors.Add(new ValidationError(
                ValidationErrorCode.NonPositiveDimension,
                $"La dimensión {field} debe ser mayor que cero (valor actual: {value}).",
                field));
        }
    }
}
