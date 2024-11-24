using GlmSharp;

namespace Helion.Render.Common.Shared;

public static class Frustum
{
    public static void SetFrustumPlanes(ref mat4 mvp, ref FrustumPlanes planes)
    {
        planes.Left.Normal.X = mvp[3] + mvp[0];
        planes.Left.Normal.Y = mvp[7] + mvp[4];
        planes.Left.Normal.Z = mvp[11] + mvp[8];
        planes.Left.Distance = mvp[15] + mvp[12];
        planes.Left.Normalize();

        planes.Right.Normal.X = mvp[3] - mvp[0];
        planes.Right.Normal.Y = mvp[7] - mvp[4];
        planes.Right.Normal.Z = mvp[11] - mvp[8];
        planes.Right.Distance = mvp[15] - mvp[12];
        planes.Right.Normalize();

        planes.Bottom.Normal.X = mvp[3] + mvp[1];
        planes.Bottom.Normal.Y = mvp[7] + mvp[5];
        planes.Bottom.Normal.Z = mvp[11] + mvp[9];
        planes.Bottom.Distance = mvp[15] + mvp[13];
        planes.Bottom.Normalize();

        planes.Top.Normal.X = mvp[3] - mvp[1];
        planes.Top.Normal.Y = mvp[7] - mvp[5];
        planes.Top.Normal.Z = mvp[11] - mvp[9];
        planes.Top.Distance = mvp[15] - mvp[13];
        planes.Top.Normalize();

        planes.Near.Normal.X = mvp[3] + mvp[2];
        planes.Near.Normal.Y = mvp[7] + mvp[6];
        planes.Near.Normal.Z = mvp[11] + mvp[10];
        planes.Near.Distance = mvp[15] + mvp[14];
        planes.Near.Normalize();

        planes.Far.Normal.X = mvp[3] - mvp[2];
        planes.Far.Normal.Y = mvp[7] - mvp[6];
        planes.Far.Normal.Z = mvp[11] - mvp[10];
        planes.Far.Distance = mvp[15] - mvp[14];
        planes.Far.Normalize();
    }
}
