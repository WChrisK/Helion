using System;
using Helion.World.Entities;
using Helion.World.Geometry.Lines;

namespace Helion.World.Physics.Blockmap;

public struct BlockmapIntersect : IComparable<BlockmapIntersect>
{
    public Entity? Entity;
    public Line? Line;
    public double SegTime;

    public BlockmapIntersect(Line line, double segTime)
    {
        Entity = null;
        Line = line;
        SegTime = segTime;
    }

    public readonly int CompareTo(BlockmapIntersect other)
    {
        return SegTime.CompareTo(other.SegTime);
    }
}
