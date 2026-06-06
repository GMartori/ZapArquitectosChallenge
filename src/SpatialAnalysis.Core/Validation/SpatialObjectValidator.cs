using SpatialAnalysis.Core.Analysis.Models;
using SpatialAnalysis.Core.Domain.Entities;
using SpatialAnalysis.Core.Domain.ValueObjects;
using SpatialAnalysis.Core.Input;
using SpatialAnalysis.Core.Validation.Rules;

namespace SpatialAnalysis.Core.Validation;

public sealed class SpatialObjectValidator
{
    private readonly IReadOnlyList<IObjectValidationRule> _rules =
    [
        new RequiredFieldsRule(),
        new InvalidNumericValuesRule(),
        new PositiveDimensionsRule(),
        new CategoryRule()
    ];

    public ProcessingBatch ValidateBatch(IReadOnlyList<SpatialObjectDto> dtos)
    {
        var evaluations = new List<RecordEvaluation>(dtos.Count);

        for (var index = 0; index < dtos.Count; index++)
        {
            var dto = dtos[index];
            var errors = new List<ValidationError>();

            foreach (var rule in _rules)
            {
                rule.Validate(dto, errors);
            }

            evaluations.Add(new RecordEvaluation(index, dto, errors));
        }

        ApplyDuplicateIdRule(evaluations);

        var validObjects = new List<SpatialObject>();
        var invalidRecords = new List<InvalidRecord>();

        foreach (var evaluation in evaluations)
        {
            if (evaluation.Errors.Count == 0)
            {
                validObjects.Add(ToSpatialObject(evaluation.Dto));
            }
            else
            {
                invalidRecords.Add(new InvalidRecord(
                    evaluation.SourceIndex,
                    evaluation.Dto,
                    evaluation.Errors));
            }
        }

        return new ProcessingBatch
        {
            TotalRead = dtos.Count,
            ValidObjects = validObjects,
            InvalidRecords = invalidRecords
        };
    }

    private static void ApplyDuplicateIdRule(List<RecordEvaluation> evaluations)
    {
        var duplicateIds = evaluations
            .GroupBy(e => e.Dto.Id)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToHashSet();

        foreach (var evaluation in evaluations)
        {
            if (!duplicateIds.Contains(evaluation.Dto.Id))
            {
                continue;
            }

            evaluation.Errors.Add(new ValidationError(
                ValidationErrorCode.DuplicateId,
                $"El Id {evaluation.Dto.Id} está duplicado en el lote.",
                nameof(SpatialObjectDto.Id)));
        }
    }

    private static SpatialObject ToSpatialObject(SpatialObjectDto dto)
    {
        var origin = new Point3D(dto.X, dto.Y, dto.Z);
        var dimensions = new Dimensions(dto.Width, dto.Height, dto.Depth);
        var box = new AxisAlignedBox(origin, dimensions);

        return new SpatialObject(
            dto.Id,
            dto.Name,
            dto.Category!,
            box);
    }

    private sealed class RecordEvaluation(int sourceIndex, SpatialObjectDto dto, List<ValidationError> errors)
    {
        public int SourceIndex { get; } = sourceIndex;
        public SpatialObjectDto Dto { get; } = dto;
        public List<ValidationError> Errors { get; } = errors;
    }
}
