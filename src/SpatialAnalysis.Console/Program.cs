using SpatialAnalysis.Console;

var dataPath = CompositionRoot.ResolveDataFilePath(args);

Console.WriteLine("Spatial Analysis — ZAP Arquitectos Challenge");
Console.WriteLine($"Data file: {dataPath}");
Console.WriteLine(File.Exists(dataPath)
    ? "Bootstrap OK. Fase 0 completada."
    : "Bootstrap OK, pero no se encontró el archivo de datos (revisar copia a output).");
