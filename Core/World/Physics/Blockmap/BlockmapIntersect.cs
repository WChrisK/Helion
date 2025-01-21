using System;
using Helion.World.Entities;
using Helion.World.Geometry.Lines;

namespace Helion.World.Physics.Blockmap;

public struct BlockmapIntersect : IComparable<BlockmapIntersect>
{
    public int? Entity;
    public int? Line;
    public double SegTime;

    public BlockmapIntersect(int lineId, double segTime)
    {
        Entity = null;
        Line = lineId;
        SegTime = segTime;
    }

    public readonly int CompareTo(BlockmapIntersect other)
    {
        return SegTime.CompareTo(other.SegTime);
    }
}
