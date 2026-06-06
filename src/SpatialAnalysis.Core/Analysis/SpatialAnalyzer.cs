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

        var isolatedObjects = FindIsolatedObjects(objects);

        return new SpatialAnalysisResult
        {
            Intersections = intersections,
            Containments = containments,
            IsolatedObjects = isolatedObjects
        };
    }

    private static IReadOnlyList<SpatialObject> FindIsolatedObjects(IReadOnlyList<SpatialObject> objects)
    {
        var isolated = new List<SpatialObject>();

        foreach (var current in objects)
        {
            var hasSpatialRelation = false;

            foreach (var other in objects)
            {
                if (current.Id == other.Id)
                {
                    continue;
                }

                if (AabbGeometry.Intersects(current.Box, other.Box)
                    || AabbGeometry.Contains(current.Box, other.Box)
                    || AabbGeometry.Contains(other.Box, current.Box))
                {
                    hasSpatialRelation = true;
                    break;
                }
            }

            if (!hasSpatialRelation)
            {
                isolated.Add(current);
            }
        }

        return isolated;
    }
}
