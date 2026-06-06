using SpatialAnalysis.Core.Domain.ValueObjects;

namespace SpatialAnalysis.Core.Geometry;

/// <summary>
/// Operaciones AABB sobre cajas alineadas a ejes (intervalos cerrados en borde).
/// </summary>
public static class AabbGeometry
{
    public static bool Intersects(AxisAlignedBox a, AxisAlignedBox b)
    {
        var overlapX = a.MinX <= b.MaxX && b.MinX <= a.MaxX;
        var overlapY = a.MinY <= b.MaxY && b.MinY <= a.MaxY;
        var overlapZ = a.MinZ <= b.MaxZ && b.MinZ <= a.MaxZ;

        return overlapX && overlapY && overlapZ;
    }

    public static bool Contains(AxisAlignedBox outer, AxisAlignedBox inner)
    {
        return inner.MinX >= outer.MinX
            && inner.MaxX <= outer.MaxX
            && inner.MinY >= outer.MinY
            && inner.MaxY <= outer.MaxY
            && inner.MinZ >= outer.MinZ
            && inner.MaxZ <= outer.MaxZ;
    }
}
