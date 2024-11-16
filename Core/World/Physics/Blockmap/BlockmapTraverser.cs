using Helion.Geometry.Boxes;
using Helion.Geometry.Grids;
using Helion.Geometry.Segments;
using Helion.Geometry.Vectors;
using Helion.Util;
using Helion.Util.Container;
using Helion.World.Blockmap;
using Helion.World.Entities;
using System;

namespace Helion.World.Physics.Blockmap;

public class BlockmapTraverser
{
    public UniformGrid<Block> BlockmapGrid;

    private IWorld m_world;
    private Block[] m_blocks;
    private int[] m_checkedLines;
    private DataCache m_dataCache;

    public BlockmapTraverser(IWorld world, BlockMap blockmap)
    {
        m_world = world;
        m_dataCache = world.DataCache;
        BlockmapGrid = blockmap.Blocks;
        m_blocks = blockmap.Blocks.Blocks;
        m_checkedLines = new int[m_world.Lines.Count];
    }

    public void UpdateTo(IWorld world, BlockMap blockmap)
    {
        m_world = world;
        m_dataCache = world.DataCache;
        BlockmapGrid = blockmap.Blocks;
        m_blocks = blockmap.Blocks.Blocks;
        if (world.Lines.Count > m_checkedLines.Length)
            m_checkedLines = new int[m_world.Lines.Count];
    }

    public void GetSolidEntityIntersections2D(Entity sourceEntity, DynamicArray<Entity> entities)
    {
        int m_checkCounter = ++WorldStatic.CheckCounter;
        var box = sourceEntity.GetBox2D();
        var it = BlockmapGrid.CreateBoxIteration(box);
        for (int by = it.BlockStart.Y; by <= it.BlockEnd.Y; by++)
        {
            for (int bx = it.BlockStart.X; bx <= it.BlockEnd.X; bx++)
            {
                Block block = m_blocks[by * it.Width + bx];
                for (int i = 0; i < block.EntityIndicesLength; i++)
                {
                    var entity = m_dataCache.Entities[block.EntityIndices[i]];
                    if (entity.BlockmapCount == m_checkCounter || !entity.Flags.Solid)
                        continue;

                    entity.BlockmapCount = m_checkCounter;
                    if (sourceEntity.CanBlockEntity(entity) && entity.Overlaps2D(box))
                        entities.Add(entity);
                }
            }
        }
    }
       

    public unsafe void SightTraverse(Seg2D seg, DynamicArray<BlockmapIntersect> intersections, out bool hitOneSidedLine)
    {
        int checkCounter = ++WorldStatic.CheckCounter;
        hitOneSidedLine = false;
        int length = 0;
        int capacity = intersections.Capacity;
        BlockmapSegIterator<Block> it = BlockmapGrid.Iterate(seg);
        var block = it.Next();
        var arrayData = intersections.Data;

        while (block != null)
        {
            int blockLineCount = block.BlockLineCount;
            if (capacity < length + blockLineCount)
            {
                intersections.EnsureCapacity(length + blockLineCount);
                capacity = intersections.Capacity;
            }

            fixed (BlockLine* lineStart = &block.BlockLines[0])
            {
                BlockLine* line = lineStart;
                for (int i = 0; i < blockLineCount; i++, line++)
                {
                    if (seg.Intersection(line->Segment.Start.X, line->Segment.Start.Y, line->Segment.End.X, line->Segment.End.Y, out double t))
                    {
                        if (m_checkedLines[line->LineId] == checkCounter)
                            continue;

                        m_checkedLines[line->LineId] = checkCounter;

                        if (line->OneSided)
                        {
                            hitOneSidedLine = true;
                            goto sightTraverseEndOfLoop;
                        }

                        if (length >= intersections.Capacity)
                        {
                            intersections.EnsureCapacity(length + 1);
                            arrayData = intersections.Data;
                        }

                        ref var bi = ref arrayData[length];
                        bi.Line = line->Line;
                        bi.Entity = null;
                        bi.SegTime = t;
                        length++;
                    }
                }
            }
            block = it.Next();
        }
        

    sightTraverseEndOfLoop:
        if (hitOneSidedLine)
            return;

        intersections.SetLength(length);
        intersections.Sort();
    }

    public unsafe void ShootTraverse(Seg2D seg, DynamicArray<BlockmapIntersect> intersections)
    {
        Vec2D intersect = Vec2D.Zero;
        int checkCounter = ++WorldStatic.CheckCounter;
        int length = 0;
        int capacity = intersections.Capacity;
        BlockmapSegIterator<Block> it = BlockmapGrid.Iterate(seg);
        var block = it.Next();
        var arrayData = intersections.Data;

        while (block != null)
        {
            fixed (BlockLine* lineStart = &block.BlockLines[0])
            {
                BlockLine* line = lineStart;
                for (int i = 0; i < block.BlockLineCount; i++, line++)
                {
                    if (seg.Intersection(line->Segment.Start.X, line->Segment.Start.Y, line->Segment.End.X, line->Segment.End.Y, out double t))
                    {
                        if (m_checkedLines[line->LineId] == checkCounter)
                            continue;

                        m_checkedLines[line->LineId] = checkCounter;

                        if (length >= intersections.Capacity)
                        {
                            intersections.EnsureCapacity(length + 1);
                            arrayData = intersections.Data;
                        }

                        ref var bi = ref arrayData[length];
                        bi.Line = line->Line;
                        bi.Entity = null;
                        bi.SegTime = t;
                        length++;
                    }
                }
            }

            for (int i = block.EntityIndicesLength - 1; i >= 0; i--)
            {
                var entity = m_dataCache.Entities[block.EntityIndices[i]];
                if (entity.BlockmapCount == checkCounter)
                    continue;
                if (!entity.Flags.Shootable)
                    continue;

                entity.BlockmapCount = checkCounter;
                if (entity.BoxIntersects(seg.Start, seg.End, ref intersect))
                {
                    if (length >= intersections.Capacity)
                    {
                        intersections.EnsureCapacity(length + 1);
                        arrayData = intersections.Data;
                    }

                    ref var bi = ref arrayData[length];
                    bi.Intersection = intersect;
                    bi.Line = null;
                    bi.Entity = entity;
                    bi.SegTime = seg.ToTime(intersect);
                    length++;
                }
            }
            block = it.Next();
        }
        
        intersections.SetLength(length);
        intersections.Sort();
    }

    public void ExplosionTraverse(Box2D box, Action<Entity> action)
    {
        int checkCounter = ++WorldStatic.CheckCounter;
        var it = BlockmapGrid.CreateBoxIteration(box);
        for (int by = it.BlockStart.Y; by <= it.BlockEnd.Y; by++)
        {
            for (int bx = it.BlockStart.X; bx <= it.BlockEnd.X; bx++)
            {
                Block block = m_blocks[by * it.Width + bx];
                for (int i = block.EntityIndicesLength - 1; i >= 0; i--)
                {
                    var entity = m_dataCache.Entities[block.EntityIndices[i]];
                    if (entity.BlockmapCount == checkCounter)
                        continue;
                    if (!entity.Flags.Shootable)
                        continue;

                    entity.BlockmapCount = checkCounter;
                    if (entity.Overlaps2D(box))
                        action(entity);
                }
            }
        }
    }

    public void EntityTraverse(Box2D box, Func<Entity, GridIterationStatus> action)
    {
        int checkCounter = ++WorldStatic.CheckCounter;
        var it = BlockmapGrid.CreateBoxIteration(box);
        for (int by = it.BlockStart.Y; by <= it.BlockEnd.Y; by++)
        {
            for (int bx = it.BlockStart.X; bx <= it.BlockEnd.X; bx++)
            {
                Block block = m_blocks[by * it.Width + bx];
                for (int i = block.EntityIndicesLength - 1; i >= 0; i--)
                {
                    var entity = m_dataCache.Entities[block.EntityIndices[i]];
                    if (entity.BlockmapCount == checkCounter)
                        continue;

                    entity.BlockmapCount = checkCounter;
                    if (!entity.Overlaps2D(box))
                        continue;

                    if (action(entity) == GridIterationStatus.Stop)
                        return;
                }
            }
        }
    }

    public void HealTraverse(Box2D box, Action<Entity> action)
    {
        int checkCounter = ++WorldStatic.CheckCounter;
        var it = BlockmapGrid.CreateBoxIteration(box);
        for (int by = it.BlockStart.Y; by <= it.BlockEnd.Y; by++)
        {
            for (int bx = it.BlockStart.X; bx <= it.BlockEnd.X; bx++)
            {
                Block block = m_blocks[by * it.Width + bx];
                for (int i = block.EntityIndicesLength - 1; i >= 0; i--)
                {
                    var entity = m_dataCache.Entities[block.EntityIndices[i]];
                    if (entity.BlockmapCount == checkCounter)
                        continue;
                    if (!entity.Flags.Corpse)
                        continue;
                    if (entity.Definition.RaiseState == null || entity.FrameState.Frame.Ticks != -1 || entity.IsPlayer)
                        continue;
                    if (m_world.IsPositionBlockedByEntity(entity, entity.Position))
                        continue;

                    entity.BlockmapCount = checkCounter;
                    if (entity.Overlaps2D(box))
                    {
                        action(entity);
                        return;
                    }
                }
            }
        }
    }

    public bool SolidBlockTraverse(Entity sourceEntity, Vec3D position, bool checkZ)
    {
        int checkCounter = ++WorldStatic.CheckCounter;
        Box3D box3D = new(position, sourceEntity.Radius, sourceEntity.Height);
        Box2D box2D = new(position.X, position.Y, sourceEntity.Radius);
        var it = BlockmapGrid.CreateBoxIteration(box2D);
        for (int by = it.BlockStart.Y; by <= it.BlockEnd.Y; by++)
        {
            for (int bx = it.BlockStart.X; bx <= it.BlockEnd.X; bx++)
            {
                Block block = m_blocks[by * it.Width + bx];
                for (int i = block.EntityIndicesLength - 1; i >= 0; i--)
                {
                    var entity = m_dataCache.Entities[block.EntityIndices[i]];
                    if (entity.BlockmapCount == checkCounter)
                        continue;
                    if (!entity.Flags.Solid)
                        continue;

                    entity.BlockmapCount = checkCounter;
                    if (!EntityOverlap(sourceEntity, entity, box3D, box2D, checkZ))
                        continue;

                    return false;
                }
            }
        }

        return true;
    }

    public void SolidBlockTraverse(Entity sourceEntity, Vec3D position, bool checkZ, DynamicArray<Entity> entities, bool shootable)
    {
        int checkCounter = ++WorldStatic.CheckCounter;
        Box3D box3D = new(position, sourceEntity.Radius, sourceEntity.Height);
        Box2D box2D = new(position.X, position.Y, sourceEntity.Radius);
        var it = BlockmapGrid.CreateBoxIteration(box2D);
        for (int by = it.BlockStart.Y; by <= it.BlockEnd.Y; by++)
        {
            for (int bx = it.BlockStart.X; bx <= it.BlockEnd.X; bx++)
            {
                Block block = m_blocks[by * it.Width + bx];
                for (int i = block.EntityIndicesLength - 1; i >= 0; i--)
                {
                    var entity = m_dataCache.Entities[block.EntityIndices[i]];
                    if (entity.BlockmapCount == checkCounter)
                        continue;
                    if (!entity.Flags.Solid)
                        continue;
                    if (shootable && !entity.Flags.Shootable)
                        continue;

                    entity.BlockmapCount = checkCounter;
                    if (!EntityOverlap(sourceEntity, entity, box3D, box2D, checkZ))
                        continue;

                    entities.Add(entity);
                }
            }
        }
    }

    private static bool EntityOverlap(Entity sourceEntity, Entity entity, in Box3D box3D, in Box2D box2D, bool checkZ)
    {
        if (!entity.Overlaps2D(box2D))
            return false;

        if (!sourceEntity.CanBlockEntity(entity))
            return false;

        if (checkZ && !entity.Overlaps(box3D))
            return false;

        if (!checkZ && !entity.Overlaps2D(box2D))
            return false;

        return true;
    }

    public unsafe void UseTraverse(Seg2D seg, DynamicArray<BlockmapIntersect> intersections)
    {
        int checkCounter = ++WorldStatic.CheckCounter;
        var it = BlockmapGrid.Iterate(seg);
        var block = it.Next();
        while (block != null)
        {
            for (int i = 0; i < block.BlockLineCount; i++)
            {
                fixed (BlockLine* line = &block.BlockLines[i])
                {
                    if (m_checkedLines[line->LineId] == checkCounter)
                        continue;

                    if (line->Segment.Intersection(seg, out double t))
                    {
                        m_checkedLines[line->LineId] = checkCounter;
                        Vec2D intersect = line->Segment.FromTime(t);
                        intersections.Add(new BlockmapIntersect(line->Line, line->Segment.FromTime(t), intersect.Distance(seg.Start)));
                    }
                }
            }
            block = it.Next();
        }
        intersections.Sort();
    }
}
