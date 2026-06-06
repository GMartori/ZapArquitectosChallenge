using SpatialAnalysis.Core.Input;

namespace SpatialAnalysis.Core.Validation.Rules;

public sealed class InvalidNumericValuesRule : IObjectValidationRule
{
    public void Validate(SpatialObjectDto dto, ICollection<ValidationError> errors)
    {
        ValidateNumber(dto.Id, nameof(SpatialObjectDto.Id), errors);
        ValidateNumber(dto.X, nameof(SpatialObjectDto.X), errors);
        ValidateNumber(dto.Y, nameof(SpatialObjectDto.Y), errors);
        ValidateNumber(dto.Z, nameof(SpatialObjectDto.Z), errors);
        ValidateNumber(dto.Width, nameof(SpatialObjectDto.Width), errors);
        ValidateNumber(dto.Height, nameof(SpatialObjectDto.Height), errors);
        ValidateNumber(dto.Depth, nameof(SpatialObjectDto.Depth), errors);
    }

    private static void ValidateNumber(double value, string field, ICollection<ValidationError> errors)
    {
        if (double.IsNaN(value) || double.IsInfinity(value))
        {
            errors.Add(new ValidationError(
                ValidationErrorCode.InvalidNumericValue,
                $"El valor numérico de {field} no es válido.",
                field));
        }
    }
}
