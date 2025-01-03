using Helion.Maps.Specials;
using Helion.World;
using Helion.World.Special.Specials;

namespace Helion.Models;

public struct SectorDamageSpecialModel
{
    public int SectorId { get; set; }
    public int Damage { get; set; }
    public int RadSuitLeak { get; set; }
    public bool End { get; set; }
    public InstantKillEffect InstantKillEffect { get; set; }

    public readonly SectorDamageSpecial? ToWorldSpecial(IWorld world)
    {
        if (!world.IsSectorIdValid(SectorId))
            return null;

        if (End)
            return new SectorDamageEndSpecial(world, world.Sectors[SectorId], this);
        else
            return new SectorDamageSpecial(world, world.Sectors[SectorId], this);
    }
}
