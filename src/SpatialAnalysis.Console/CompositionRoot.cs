namespace SpatialAnalysis.Console;

/// <summary>
/// Punto de composición manual (sin contenedor DI).
/// </summary>
internal static class CompositionRoot
{
    public static string ResolveDataFilePath(string[] args)
    {
        if (args.Length > 0 && !string.IsNullOrWhiteSpace(args[0]))
        {
            return Path.GetFullPath(args[0]);
        }

        return Path.Combine(AppContext.BaseDirectory, "data", "objects.json");
    }
}
