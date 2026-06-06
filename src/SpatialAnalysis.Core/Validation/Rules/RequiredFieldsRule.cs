using SpatialAnalysis.Core.Input;

namespace SpatialAnalysis.Core.Validation.Rules;

public sealed class RequiredFieldsRule : IObjectValidationRule
{
    public void Validate(SpatialObjectDto dto, ICollection<ValidationError> errors)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            errors.Add(new ValidationError(
                ValidationErrorCode.MissingRequiredField,
                "El campo Name es obligatorio.",
                nameof(SpatialObjectDto.Name)));
        }
    }
}
