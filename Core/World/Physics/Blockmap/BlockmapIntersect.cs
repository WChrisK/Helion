using System;
namespace Helion.World.Physics.Blockmap;

public struct BlockmapIntersect : IComparable<BlockmapIntersect>
{
    public const int EntityFlag = 1 << 31;
    public const int EntityMask = ~EntityFlag;

    public int Index;
    public float SegTime;

    public BlockmapIntersect(int lineId, double segTime)
    {
        Index = lineId;
        SegTime = (float)segTime;
    }

    public readonly bool GetLineIndex(out int index)
    {
        if ((Index & EntityFlag) == 0)
        {
            index = Index;
            return true;
        }

        index = Index & EntityMask;
        return false;
    }

    public readonly int CompareTo(BlockmapIntersect other)
    {
        return SegTime.CompareTo(other.SegTime);
    }
}
