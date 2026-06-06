using SpatialAnalysis.Console;
using SpatialAnalysis.Console.Reporting;
using SpatialAnalysis.Core.Parsing;

try
{
    var dataPath = CompositionRoot.ResolveDataFilePath(args);
    var reader = new SpatialObjectJsonReader();
    var objects = reader.ReadFromFile(dataPath);

    ObjectListingWriter.WriteSummary(Console.Out, dataPath, objects);
    return 0;
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Error: {ex.Message}");
    return 1;
}
