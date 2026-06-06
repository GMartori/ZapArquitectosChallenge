using SpatialAnalysis.Console;
using SpatialAnalysis.Console.Reporting;
using SpatialAnalysis.Core.Analysis;
using SpatialAnalysis.Core.Parsing;
using SpatialAnalysis.Core.Validation;

try
{
    var dataPath = CompositionRoot.ResolveDataFilePath(args);
    var reader = new SpatialObjectJsonReader();
    var dtos = reader.ReadFromFile(dataPath);

    var validator = new SpatialObjectValidator();
    var batch = validator.ValidateBatch(dtos);

    var analyzer = new SpatialAnalyzer();
    var spatialResult = analyzer.Analyze(batch.ValidObjects);

    ValidationReportWriter.Write(Console.Out, dataPath, batch, spatialResult);
    return 0;
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Error: {ex.Message}");
    return 1;
}
