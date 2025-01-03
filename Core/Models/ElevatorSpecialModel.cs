﻿using Helion.World;
using Helion.World.Special;
using Helion.World.Special.Specials;

namespace Helion.Models;

public struct ElevatorSpecialModel : ISpecialModel
{
    public SectorMoveSpecialModel FirstMove { get; set; }
    public SectorMoveSpecialModel SecondMove { get; set; }

    public readonly ISpecial? ToWorldSpecial(IWorld world)
    {
        if (!world.IsSectorIdValid(FirstMove.SectorId) || !world.IsSectorIdValid(SecondMove.SectorId))
            return null;

        var firstMoveSpecial = FirstMove.ToWorldSpecial(world);
        var secondMoveSpecial = SecondMove.ToWorldSpecial(world);

        if (firstMoveSpecial is not SectorMoveSpecial firstWorldMoveSpecial)
            return null;
        if (secondMoveSpecial is not SectorMoveSpecial secondWorldMoveSpecial)
            return null;

        return new ElevatorSpecial(world.Sectors[firstWorldMoveSpecial.Sector.Id], firstWorldMoveSpecial, secondWorldMoveSpecial);
    }
}
