using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Helion.Geometry;
using Helion.Geometry.Boxes;
using Helion.Geometry.Grids;
using Helion.Geometry.Segments;
using Helion.Geometry.Vectors;
using Helion.Util.Assertion;
using Helion.Util.Container;
using Helion.World.Entities;
using Helion.World.Geometry.Lines;
using Helion.World.Geometry.Sectors;
using Helion.World.Geometry.Sides;

namespace Helion.World.Blockmap;

/// <summary>
/// A conversion of a map into a grid structure whereby things in certain
/// blocks only will check the blocks they are in for collision detection
/// or line intersections to optimize computational cost.
/// </summary>
public class BlockMap
{
    public readonly Box2D Bounds;
    private readonly UniformGrid<Block> m_blocks;
    public UniformGrid<Block> Blocks => m_blocks;
    
    public BlockMap(IList<Line> lines, int blockDimension)
    {
        Bounds = FindMapBoundingBox(lines) ?? new Box2D(Vec2D.Zero, Vec2D.One);
        m_blocks = new UniformGrid<Block>(Bounds, blockDimension);
        SetBlockCoordinates();
        AddLinesToBlocks(lines);
    }

    public unsafe void Clear()
    {
        foreach (var block in m_blocks.Blocks)
        {
            // Note: Entities are unlinked using UnlinkFromWorld. Only need to dump the other data.
            
            var islandNode = block.DynamicSectors.Head;
            while (islandNode != null)
            {
                var nextNode = islandNode.Next;
                islandNode.Unlink();
                WorldStatic.DataCache.FreeLinkableNodeIsland(islandNode);
                islandNode = nextNode;
            }

            block.DynamicSides.Clear();
        }
    }

    public BlockMap(Box2D bounds, int blockDimension)
    {
        Bounds = bounds;
        m_blocks = new UniformGrid<Block>(Bounds, blockDimension);
        SetBlockCoordinates();
    }

    public void Dispose()
    {
        for (int i = 0; i < m_blocks.Blocks.Length; i++)
        {
            var block = m_blocks.Blocks[i];
            for (int j = 0; j < block.BlockLineCount; j++)
                block.BlockLines[j] = default;
        }
    }
    
    public BlockmapSegIterator<Block> Iterate(in Seg2D seg)
    {
        return m_blocks.Iterate(seg);
    }

    /// <summary>
    /// Links an entity to the grid.
    /// </summary>
    /// <param name="entity">The entity to link. Should be inside the map.
    /// </param>
    public void Link(Entity entity)
    {
        Assert.Precondition(entity.BlockmapNodes.Empty(), "Forgot to unlink entity from blockmap");

        var it = m_blocks.CreateBoxIteration(entity.GetBox2D());
        for (int by = it.BlockStart.Y; by <= it.BlockEnd.Y; by++)
        {
            for (int bx = it.BlockStart.X; bx <= it.BlockEnd.X; bx++)
            {
                Block block = m_blocks[by * it.Width + bx];
                LinkableNode<Entity> blockEntityNode = WorldStatic.DataCache.GetLinkableNodeEntity(entity);
                block.Entities.Add(blockEntityNode);
                entity.BlockmapNodes.Add(blockEntityNode);
            }
        }
    }

    public void RenderLink(Entity entity)
    {
        Assert.Precondition(entity.RenderBlock == null, "Forgot to unlink entity from render blockmap");
        entity.RenderBlock = m_blocks.GetBlock(entity.Position);
        if (entity.RenderBlock == null)
            return;

        entity.RenderBlock.AddLink(entity);
    }

    public void Link(IWorld world, Sector sector)
    {
        Assert.Precondition(sector.BlockmapNodes.Empty(), "Forgot to unlink sector from blockmap");

        var islands = world.Geometry.IslandGeometry.SectorIslands[sector.Id];
        foreach (var sectorIsland in islands)
        {
            if (sectorIsland.IsVooDooCloset || sectorIsland.IsMonsterCloset)
                continue;
            var it = m_blocks.CreateBoxIteration(sectorIsland.Box);
            for (int by = it.BlockStart.Y; by <= it.BlockEnd.Y; by++)
            {
                for (int bx = it.BlockStart.X; bx <= it.BlockEnd.X; bx++)
                {
                    Block block = m_blocks[by * it.Width + bx];
                    var node = world.DataCache.GetLinkableNodeIsland(sectorIsland);
                    block.DynamicSectors.Add(node);
                    sector.BlockmapNodes.Add(node);
                }
            }
        }
    }

    public void LinkDynamicSide(Side side)
    {
        if (side.BlockmapLinked)
            return;

        side.BlockmapLinked = true;
        
        BlockmapSegIterator<Block> it = m_blocks.Iterate(side.Line.Segment);
        var block = it.Next();
        while (block != null)
        {
            block.DynamicSides.Add(side);
            block = it.Next();
        }
    }

    private static Box2D? FindMapBoundingBox(IEnumerable<Line> lines)
    {
        var boxes = lines.Select(l => l.Segment.Box);
        return Box2D.Combine(boxes);
    }

    private void SetBlockCoordinates()
    {
        // Unfortunately we have to do it this way because we can't get
        // constraining for generic parameters, so the UniformGrid will
        // not be able to do this for us via it's constructor.
        int index = 0;
        for (int y = 0; y < m_blocks.Height; y++)
            for (int x = 0; x < m_blocks.Width; x++)
                m_blocks[index++].SetCoordinate(x, y, m_blocks.Dimension, m_blocks.Origin);
    }

    private void AddLinesToBlocks(IList<Line> lines)
    {
        foreach (Line line in lines)
        {
            m_blocks.Iterate(line.Segment, block =>
            {
                if (block.BlockLines.Length == block.BlockLineCount)
                {
                    var newLines = new BlockLine[block.BlockLines.Length * 2];
                    Array.Copy(block.BlockLines, newLines, block.BlockLines.Length);
                    block.BlockLines = newLines;
                }

                block.BlockLines[block.BlockLineCount++] = new BlockLine(line.Segment, line, line.Back == null, line.Front.Sector, line.Back?.Sector);
                return GridIterationStatus.Continue;
            });
        }
    }
}
