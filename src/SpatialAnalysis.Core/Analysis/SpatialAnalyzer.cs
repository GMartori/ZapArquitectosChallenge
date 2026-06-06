using SpatialAnalysis.Core.Analysis.Models;
using SpatialAnalysis.Core.Domain.Entities;
using SpatialAnalysis.Core.Geometry;

namespace SpatialAnalysis.Core.Analysis;

public sealed class SpatialAnalyzer
{
    public SpatialAnalysisResult Analyze(IReadOnlyList<SpatialObject> objects)
    {
        var intersections = new List<ObjectPair>();
        var containments = new List<ContainmentPair>();

        for (var i = 0; i < objects.Count; i++)
        {
            for (var j = i + 1; j < objects.Count; j++)
            {
                var a = objects[i];
                var b = objects[j];

                if (AabbGeometry.Intersects(a.Box, b.Box))
                {
                    intersections.Add(new ObjectPair(a, b));
                }

                if (AabbGeometry.Contains(a.Box, b.Box))
                {
                    containments.Add(new ContainmentPair(b, a));
                }
                else if (AabbGeometry.Contains(b.Box, a.Box))
                {
                    containments.Add(new ContainmentPair(a, b));
                }
            }
        }

        return new SpatialAnalysisResult
        {
            Intersections = intersections,
            Containments = containments
        };
    }
}
