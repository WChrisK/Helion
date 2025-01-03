using Helion.World;
using Helion.World.Special;
using Helion.World.Special.Specials;

namespace Helion.Models;

public struct LightStrobeSpecialModel : ISpecialModel
{
    public int SectorId { get; set; }
    public short Max { get; set; }
    public short Min { get; set; }
    public int BrightTics { get; set; }
    public int DarkTics { get; set; }
    public int Delay { get; set; }

    public readonly ISpecial? ToWorldSpecial(IWorld world)
    {
        if (!world.IsSectorIdValid(SectorId))
            return null;

        return new LightStrobeSpecial(world, world.Sectors[SectorId], this);
    }
}
