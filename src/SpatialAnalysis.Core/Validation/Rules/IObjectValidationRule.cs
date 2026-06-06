using SpatialAnalysis.Core.Input;

namespace SpatialAnalysis.Core.Validation.Rules;

public interface IObjectValidationRule
{
    void Validate(SpatialObjectDto dto, ICollection<ValidationError> errors);
}
