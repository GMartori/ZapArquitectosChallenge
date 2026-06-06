using SpatialAnalysis.Core.Input;

namespace SpatialAnalysis.Core.Validation.Rules;

public sealed class CategoryRule : IObjectValidationRule
{
    public void Validate(SpatialObjectDto dto, ICollection<ValidationError> errors)
    {
        if (dto.Category is null)
        {
            errors.Add(new ValidationError(
                ValidationErrorCode.MissingCategory,
                "La categoría no puede ser nula.",
                nameof(SpatialObjectDto.Category)));
            return;
        }

        if (string.IsNullOrWhiteSpace(dto.Category))
        {
            errors.Add(new ValidationError(
                ValidationErrorCode.MissingCategory,
                "La categoría no puede estar vacía.",
                nameof(SpatialObjectDto.Category)));
        }
    }
}
