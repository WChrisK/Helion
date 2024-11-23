using Helion.Geometry.Boxes;
using Helion.Geometry.Vectors;
using System.Runtime.CompilerServices;

namespace Helion.Render.Common.Shared;

public struct FrustumPlanes
{
    public FrustumPlane Left;
    public FrustumPlane Right;
    public FrustumPlane Top;
    public FrustumPlane Bottom;
    public FrustumPlane Near;
    public FrustumPlane Far;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool BoxInFrustum(in Box2D box)
    {
        return BoxInPlane(Left, box.Min, box.Max) && BoxInPlane(Right, box.Min, box.Max);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool BoxInFront(in Box2D box)
    {
        return BoxInPlane(Near, box.Min, box.Max);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool PointInFrustum(double x, double y)
    {
        return PointInPlane(Left, x, y) && PointInPlane(Right, x, y);
    }

    private static bool BoxInPlane(in FrustumPlane plane, in Vec2D min, in Vec2D max)
    {
        if (PointInPlane(plane, min.X, min.Y))
            return true;

        if (PointInPlane(plane, max.X, max.Y))
            return true;

        if (PointInPlane(plane, min.X, max.Y))
            return true;

        if (PointInPlane(plane, max.X, min.Y))
            return true;

        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool SphereInFrustum(in Vec3D pos, double radius)
    {
        return SphereInPlane(Left, pos, radius) && SphereInPlane(Right, pos, radius) &&
            SphereInPlane(Top, pos, radius) && SphereInPlane(Bottom, pos, radius);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool SphereInPlane(in FrustumPlane plane, in Vec3D pos, double radius)
    {
        return plane.Normal.X * pos.X + 
            plane.Normal.Y * pos.Y +
            plane.Normal.Z * pos.Z + 
            plane.Distance > -radius;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool PointInPlane(in FrustumPlane plane, double x, double y)
    {
        var dist = (plane.Normal.X * x) + (plane.Normal.Y * y) + plane.Distance;
        return dist >= 0;
    }
}

public struct FrustumPlane(Vec3D normal, double distance)
{
    public Vec3D Normal = normal;
    public double Distance = distance;

    public void Normalize()
    {
        var length = Normal.Length();
        Normal.X /= length;
        Normal.Y /= length;
        Normal.Z /= length;
        Distance /= length;
    }
}
