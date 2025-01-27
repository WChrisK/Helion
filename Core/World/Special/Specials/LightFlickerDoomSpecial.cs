using Helion.Models;
using Helion.Util.RandomGenerators;
using Helion.World.Geometry.Sectors;

namespace Helion.World.Special.Specials;

public class LightFlickerDoomSpecial : SectorSpecialBase
{
    public readonly short MaxBright;
    public readonly short MinBright;
    public int Delay { get; private set; }

    public override bool OverrideEquals => true;

    private readonly IRandom m_random;

    public LightFlickerDoomSpecial(IWorld world, Sector sector, IRandom random, short minLightLevel)
         : base(world, sector)
    {
        m_random = random;
        MaxBright = sector.LightLevel;
        MinBright = minLightLevel;
    }

    public LightFlickerDoomSpecial(IWorld world, Sector sector, IRandom random, in LightFlickerDoomSpecialModel model)
         : base(world, sector)
    {
        m_random = random;
        MaxBright = model.Max;
        MinBright = model.Min;
        Delay = model.Delay;
    }

    public LightFlickerDoomSpecialModel ToSpecialModel()
    {
        return new()
        {
            SectorId = Sector.Id,
            Max = MaxBright,
            Min = MinBright,
            Delay = Delay
        };
    }

    public override SpecialTickStatus Tick()
    {
        if (Delay > 0)
        {
            Delay--;
            return SpecialTickStatus.Continue;
        }

        if (Sector.LightLevel == MaxBright)
        {
            World.SetSectorLightLevel(Sector, MinBright);
            Delay = (m_random.NextByte() & 7) + 1;
        }
        else
        {
            World.SetSectorLightLevel(Sector, MaxBright);
            Delay = (m_random.NextByte() & 31) + 1;
        }

        return SpecialTickStatus.Continue;
    }

    public virtual SectorBaseSpecialType SectorBaseSpecialType => SectorBaseSpecialType.Light;

    public override bool Equals(object? obj)
    {
        if (obj is not LightFlickerDoomSpecial light)
            return false;

        return light.Sector.Id == Sector.Id &&
            light.MinBright == MinBright &&
            light.MaxBright == MaxBright &&
            light.Delay == Delay;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
