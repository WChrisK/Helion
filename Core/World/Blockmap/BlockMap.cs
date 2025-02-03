using System;
using System.Collections.Generic;
using System.Linq;
using Helion.Geometry.Boxes;
using Helion.Geometry.Grids;
using Helion.Geometry.Segments;
using Helion.Geometry.Vectors;
using Helion.Util;
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
    private readonly UniformGrid m_blocks;
    public UniformGrid Blocks => m_blocks;
    public BlockLine[] BlockLines;
    public int BlockLineCount;
    
    public BlockMap(IList<Line> lines, int blockDimension)
    {
        BlockLines = new BlockLine[lines.Count];
        Bounds = FindMapBoundingBox(lines) ?? new Box2D(Vec2D.Zero, Vec2D.One);
        m_blocks = new UniformGrid(Bounds, blockDimension);
        SetBlockCoordinates();
        AddLinesToBlocks(lines);
    }

    public void Clear()
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
        BlockLines = [];
        Bounds = bounds;
        m_blocks = new UniformGrid(Bounds, blockDimension);
        SetBlockCoordinates();
    }   

    public void Link(Entity entity, bool checkLastBlock)
    {
        Assert.Precondition((entity.BlockRange.StartX == Constants.ClearBlock) || checkLastBlock, "Forgot to unlink entity from blockmap");

        var boxMinX = entity.Position.X - entity.Radius;
        var boxMaxX = entity.Position.X + entity.Radius;
        var boxMinY = entity.Position.Y - entity.Radius;
        var boxMaxY = entity.Position.Y + entity.Radius;
        var blockStartX = (short)Math.Max(0, (int)((boxMinX - m_blocks.Bounds.Min.X) / m_blocks.Dimension));
        var blockStartY = (short)Math.Max(0, (int)((boxMinY - m_blocks.Bounds.Min.Y) / m_blocks.Dimension));
        var blockEndX = (short)Math.Min((int)((boxMaxX - m_blocks.Bounds.Min.X) / m_blocks.Dimension), m_blocks.Width - 1);
        var blockEndY = (short)Math.Min((int)((boxMaxY - m_blocks.Bounds.Min.Y) / m_blocks.Dimension), m_blocks.Height - 1);

        // If the block range matches then the entity will link to the same blocks.
        // The block stores entities by id in an array so this saves the array copy to remove the index.
        if (checkLastBlock && entity.BlockRange.StartX == blockStartX && entity.BlockRange.StartY == blockStartY && entity.BlockRange.EndX == blockEndX && entity.BlockRange.EndY == blockEndY)
            return;

        entity.UnlinkBlockMapBlocks();
        entity.BlockRange.StartX = blockStartX;
        entity.BlockRange.StartY = blockStartY;
        entity.BlockRange.EndX = blockEndX;
        entity.BlockRange.EndY =  blockEndY;

        for (var by = blockStartY; by <= blockEndY; by++)
        {
            for (var bx = blockStartX; bx <= blockEndX; bx++)
            {
                var block = Blocks[by * m_blocks.Width + bx];
                
                if (block.EntityIndicesLength == block.EntityIndices.Length)                
                    Array.Resize(ref block.EntityIndices, block.EntityIndices.Length * 2);

                block.EntityIndices[block.EntityIndicesLength++] = entity.Index;
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

    public void LinkDynamic(IWorld world, Sector sector)
    {
        Assert.Precondition(sector.BlockmapNodes.Empty(), "Forgot to unlink sector from blockmap");

        var islands = world.Geometry.IslandGeometry.SectorIslands[sector.Id];
        foreach (var sectorIsland in islands)
        {
            if (sectorIsland.IsVooDooCloset || sectorIsland.IsMonsterCloset)
                continue;
            var it = m_blocks.CreateBoxIteration(sectorIsland.Box);
            for (int by = it.BlockStartY; by <= it.BlockEndY; by++)
            {
                for (int bx = it.BlockStartX; bx <= it.BlockEndX; bx++)
                {
                    Block block = m_blocks[by * it.Width + bx];
                    var node = world.DataCache.GetLinkableNodeIsland(sectorIsland);
                    block.DynamicSectors.Add(node);
                    sector.BlockmapNodes.Add(node);
                }
            }
        }
    }

    public void Link(IWorld world, Sector sector)
    {
        var islands = world.Geometry.IslandGeometry.SectorIslands[sector.Id];
        foreach (var sectorIsland in islands)
        {
            if (sectorIsland.IsVooDooCloset || sectorIsland.IsMonsterCloset)
                continue;
            var it = m_blocks.CreateBoxIteration(sectorIsland.Box);
            for (int by = it.BlockStartY; by <= it.BlockEndY; by++)
            {
                for (int bx = it.BlockStartX; bx <= it.BlockEndX; bx++)
                {
                    Block block = m_blocks[by * it.Width + bx];
                    block.Sectors.Add(new() { Value = sectorIsland });
                }
            }
        }
    }

    public void LinkDynamicSide(Side side)
    {
        if (side.BlockmapLinked)
            return;

        side.BlockmapLinked = true;
        
        var it = new BlockmapSegIterator(m_blocks, side.Line.Segment);
        while (true)
        {
            var block = it.Next();
            if (block == null)
                break;
            block.DynamicSides.Add(side);
        }
    }

    private static Box2D? FindMapBoundingBox(IList<Line> lines)
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
        for (int i = 0; i < lines.Count; i++)
        {
            var line = lines[i];
            var it = new BlockmapSegIterator(m_blocks, line.Segment);
            while (true)
            {
                var block = it.Next();
                if (block == null)
                    break;

                if (BlockLineCount == BlockLines.Length)
                {
                    var newLines = new BlockLine[(int)(BlockLines.Length * 1.5)];
                    Array.Copy(BlockLines, newLines, BlockLineCount);
                    BlockLines = newLines;
                }

                var index = block.Y * m_blocks.Width + block.X;
                BlockLines[BlockLineCount++] = new BlockLine(index, line.Segment, line, line.Back == null, line.Front.Sector, line.Back?.Sector);
            }
        }

        Array.Sort(BlockLines, 0, BlockLineCount, null);
        SetBlockLineIndices();
    }

    private void SetBlockLineIndices()
    {
        int lastIndex = -1;
        var block = m_blocks.Blocks[0];

        for (int i = 0; i < BlockLineCount; i++)
        {
            ref var blockLine = ref BlockLines[i];
            if (blockLine.BlockIndex != lastIndex)
            {
                lastIndex = blockLine.BlockIndex;
                block = m_blocks[blockLine.BlockIndex];
                block.BlockLineIndex = i;
            }

            block.BlockLineCount++;
        }
    }
}
