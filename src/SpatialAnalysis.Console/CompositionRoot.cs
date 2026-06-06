namespace SpatialAnalysis.Console;

/// <summary>
/// Punto de composición manual (sin contenedor DI).
/// </summary>
internal static class CompositionRoot
{
    public static string ResolveDataFilePath(string[] args)
    {
        for (var i = 0; i < args.Length; i++)
        {
            if (IsExportFlag(args[i]))
            {
                i++;
                continue;
            }

            if (IsFlag(args[i]))
            {
                continue;
            }

            return Path.GetFullPath(args[i]);
        }

        return Path.Combine(AppContext.BaseDirectory, "data", "objects.json");
    }

    public static string ResolveExportFilePath(string[] args)
    {
        for (var i = 0; i < args.Length - 1; i++)
        {
            if (IsExportFlag(args[i]))
            {
                return Path.GetFullPath(args[i + 1]);
            }
        }

        return Path.Combine(AppContext.BaseDirectory, "output", "results.txt");
    }

    public static bool ShouldExport(string[] args) =>
        !HasFlag(args, "--no-export");

    private static bool IsFlag(string value) =>
        value.StartsWith('-');

    private static bool IsExportFlag(string value) =>
        value is "--output" or "-o";

    private static bool HasFlag(string[] args, string flag) =>
        args.Any(arg => string.Equals(arg, flag, StringComparison.OrdinalIgnoreCase));
}
