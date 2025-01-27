using Helion.Models;
using Helion.World.Entities;
using Helion.World.Geometry.Sectors;
using System;
using System.Collections.Generic;

namespace Helion.World.Special;

public abstract class SectorSpecialBase : ISectorSpecial
{
    public Sector Sector { get; set; }
    public IWorld World { get; set; }

    public SectorSpecialBase(IWorld world, Sector sector)
    {
        World = world;
        Sector = sector;
    }

    public abstract SpecialTickStatus Tick();

    public virtual void FinalizeDestroy()
    {
        // Unused
    }

    public virtual void Free()
    {
        // Unused
    }

    public virtual void Pause()
    {
        // Unused
    }

    public virtual void Resume()
    {
        // Unused
    }

    public virtual bool Use(Entity entity)
    {
        return false;
    }

    public virtual bool IsPaused { get; }
    public virtual bool OverrideEquals => false;

    public virtual bool MultiSector => false;
    public virtual void GetSectors(List<(Sector, SectorPlane)> data) { }
}
