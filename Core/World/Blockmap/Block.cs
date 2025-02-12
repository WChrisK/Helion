using Helion.Geometry.Boxes;
using Helion.Geometry.Vectors;
using Helion.World.Entities;
using static Helion.Util.Assertion.Assert;

namespace Helion.World.Blockmap;

public class Block
{
    public int BlockLineIndex;
    public int BlockLineCount;

    public Entity? HeadEntity;

    public Box2D Box;

    /// <summary>
    /// Sets the internal coordinates for this block.
    /// </summary>
    /// <remarks>
    /// We are stuck with this because we can't do this in the constructor
    /// as this is passed in as a generic value to a UniformGrid. The only
    /// other way is to make it have some kind of interface and constrain
    /// it to that, but performance needs to be investigated first before
    /// doing that.
    /// </remarks>
    /// <param name="x">The X coordinate, which should not be negative.
    /// </param>
    /// <param name="y">The Y coordinate, which should not be negative.
    /// </param>
    internal void SetCoordinate(int x, int y, int dimension, Vec2D origin)
    {
        Precondition(x >= 0, "Cannot have a negative blockmap X index");
        Precondition(y >= 0, "Cannot have a negative blockmap Y index");

        Vec2D point = new Vec2D(x * dimension, y * dimension) + origin;
        Box = new(point, point + (dimension, dimension));
    }

    public void AddLink(Entity entity)
    {
        if (HeadEntity == null)
        {
            HeadEntity = entity;
            return;
        }

        entity.RenderBlockNext = HeadEntity;
        HeadEntity.RenderBlockPrevious = entity;
        HeadEntity = entity;
    }

    public void RemoveLink(Entity entity)
    {
        if (entity == HeadEntity)
        {
            HeadEntity = entity.RenderBlockNext;
            if (HeadEntity != null)
                HeadEntity.RenderBlockPrevious = null;
            entity.RenderBlockNext = null;
            entity.RenderBlockPrevious = null;
            return;
        }

        if (entity.RenderBlockNext != null)
            entity.RenderBlockNext.RenderBlockPrevious = entity.RenderBlockPrevious;
        if (entity.RenderBlockPrevious != null)
            entity.RenderBlockPrevious.RenderBlockNext = entity.RenderBlockNext;

        entity.RenderBlockNext = null;
        entity.RenderBlockPrevious = null;
    }
}
