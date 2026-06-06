namespace SpatialAnalysis.Core.Domain.ValueObjects;

public readonly record struct AxisAlignedBox(Point3D Origin, Dimensions Dimensions)
{
    public double MinX => Origin.X;
    public double MinY => Origin.Y;
    public double MinZ => Origin.Z;
    public double MaxX => Origin.X + Dimensions.Width;
    public double MaxY => Origin.Y + Dimensions.Height;
    public double MaxZ => Origin.Z + Dimensions.Depth;
}
