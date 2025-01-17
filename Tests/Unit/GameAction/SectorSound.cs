﻿using FluentAssertions;
using Helion.Geometry.Vectors;
using Helion.Resources.IWad;
using Helion.World.Cheats;
using Helion.World.Entities.Players;
using Helion.World.Impl.SinglePlayer;
using Xunit;

namespace Helion.Tests.Unit.GameAction
{
    [Collection("GameActions")]
    public class SectorSound
    {
        private readonly SinglePlayerWorld World;
        private Player Player => World.Player;

        public SectorSound()
        {
            World = WorldAllocator.LoadMap("Resources/sectorsound.zip", "sectorsound.WAD", "MAP01", GetType().Name, WorldInit, IWadType.Doom2);
            ClearSoundTargets();
            GameActions.GetSector(World, 6).Ceiling.SetZ(0);
            GameActions.GetSector(World, 8).Ceiling.SetZ(32);
            GameActions.GetSector(World, 10).Floor.SetZ(64);
            GameActions.DestroyCreatedEntities(World);
        }

        private void WorldInit(SinglePlayerWorld world)
        {
            world.CheatManager.ActivateCheat(world.Player, CheatType.God);
        }

        private void ClearSoundTargets()
        {
            foreach (var sector in World.Sectors)
                sector.SoundTarget = new(null);
        }

        [Fact(DisplayName = "Sound from sector 0")]
        public void SoundSector0()
        {
            GameActions.SetEntityPosition(World, Player, new Vec2D(-192, -448));
            World.NoiseAlert(Player, Player);
            GameActions.GetSector(World, 0).SoundTarget.Get().Should().Be(Player);
            GameActions.GetSector(World, 2).SoundTarget.Get().Should().Be(Player);
            GameActions.GetSector(World, 5).SoundTarget.Get().Should().Be(Player);
            // Blocked by block sound line
            GameActions.GetSector(World, 3).SoundTarget.Get().Should().BeNull();

            // Closed door
            GameActions.GetSector(World, 6).SoundTarget.Get().Should().BeNull();
            GameActions.GetSector(World, 4).SoundTarget.Get().Should().BeNull();

            // Closed sector
            GameActions.GetSector(World, 7).SoundTarget.Get().Should().BeNull();
            GameActions.GetSector(World, 8).SoundTarget.Get().Should().BeNull();

            // Closed sector
            GameActions.GetSector(World, 10).SoundTarget.Get().Should().BeNull();
            GameActions.GetSector(World, 11).SoundTarget.Get().Should().BeNull();

            // Completely separate
            GameActions.GetSector(World, 1).SoundTarget.Get().Should().BeNull();
        }

        [Fact(DisplayName = "Sound from sector 0, monsters set target")]
        public void SoundMonsterTargetSector0()
        {
            GameActions.SetEntityPosition(World, Player, new Vec2D(-192, -448));
            var imp1 = GameActions.CreateEntity(World, "DoomImp", new Vec3D(-192, 64, 0), frozen: false);
            var imp2 = GameActions.CreateEntity(World, "DoomImp", new Vec3D(0, 64, 0), frozen: false);
            var imp3 = GameActions.CreateEntity(World, "DoomImp", new Vec3D(192, -480, 0), frozen: false);
            var imp4 = GameActions.CreateEntity(World, "DoomImp", new Vec3D(-768, -288, 0), frozen: false);

            World.NoiseAlert(Player, Player);
            GameActions.TickWorld(World, 10);

            imp1.Target.Get().Should().Be(Player);
            imp2.Target.Get().Should().Be(Player);

            // Blocked by block sound line
            imp3.Target.Get().Should().BeNull();

            // Separated
            imp4.Target.Get().Should().BeNull();
        }

        [Fact(DisplayName = "Sound from sector 2")]
        public void SoundSector2()
        {
            GameActions.SetEntityPosition(World, Player, new Vec2D(0, 64));
            World.NoiseAlert(Player, Player);
            GameActions.GetSector(World, 0).SoundTarget.Get().Should().Be(Player);
            GameActions.GetSector(World, 2).SoundTarget.Get().Should().Be(Player);
            GameActions.GetSector(World, 5).SoundTarget.Get().Should().Be(Player);

            // No longer blocked
            GameActions.GetSector(World, 3).SoundTarget.Get().Should().Be(Player);

            // Closed door
            GameActions.GetSector(World, 6).SoundTarget.Get().Should().BeNull();
            GameActions.GetSector(World, 4).SoundTarget.Get().Should().BeNull();

            // Closed sector
            GameActions.GetSector(World, 7).SoundTarget.Get().Should().BeNull();
            GameActions.GetSector(World, 8).SoundTarget.Get().Should().BeNull();

            // Closed sector
            GameActions.GetSector(World, 10).SoundTarget.Get().Should().BeNull();
            GameActions.GetSector(World, 11).SoundTarget.Get().Should().BeNull();

            // Completely separate
            GameActions.GetSector(World, 1).SoundTarget.Get().Should().BeNull();
        }

        [Fact(DisplayName = "Sound from sector 2 with open door")]
        public void SoundOpenDoorSector2()
        {
            GameActions.GetSector(World, 6).Ceiling.SetZ(56);
            GameActions.GetSector(World, 8).Ceiling.SetZ(56);
            GameActions.GetSector(World, 10).Floor.SetZ(10);

            GameActions.SetEntityPosition(World, Player, new Vec2D(0, 64));
            World.NoiseAlert(Player, Player);
            GameActions.GetSector(World, 0).SoundTarget.Get().Should().Be(Player);
            GameActions.GetSector(World, 2).SoundTarget.Get().Should().Be(Player);
            GameActions.GetSector(World, 5).SoundTarget.Get().Should().Be(Player);
            // No longer blocked
            GameActions.GetSector(World, 3).SoundTarget.Get().Should().Be(Player);

            // Door is now open
            GameActions.GetSector(World, 6).SoundTarget.Get().Should().Be(Player);
            GameActions.GetSector(World, 4).SoundTarget.Get().Should().Be(Player);

            // Closed sector is now open
            GameActions.GetSector(World, 7).SoundTarget.Get().Should().Be(Player);
            GameActions.GetSector(World, 8).SoundTarget.Get().Should().Be(Player);

            // Closed sector is now open
            GameActions.GetSector(World, 10).SoundTarget.Get().Should().Be(Player);
            GameActions.GetSector(World, 11).SoundTarget.Get().Should().Be(Player);

            // Completely separate
            GameActions.GetSector(World, 1).SoundTarget.Get().Should().BeNull();
        }

        [Fact(DisplayName = "Sound from sector 4 with closed door")]
        public void SoundSector4()
        {
            GameActions.SetEntityPosition(World, Player, new Vec2D(0, 480));
            World.NoiseAlert(Player, Player);

            // Sector 4 is completely closed off by the door
            GameActions.GetSector(World, 4).SoundTarget.Get().Should().Be(Player);

            GameActions.GetSector(World, 0).SoundTarget.Get().Should().BeNull();
            GameActions.GetSector(World, 2).SoundTarget.Get().Should().BeNull();
            GameActions.GetSector(World, 5).SoundTarget.Get().Should().BeNull();
            GameActions.GetSector(World, 3).SoundTarget.Get().Should().BeNull();
            GameActions.GetSector(World, 6).SoundTarget.Get().Should().BeNull();
            GameActions.GetSector(World, 1).SoundTarget.Get().Should().BeNull();
            GameActions.GetSector(World, 7).SoundTarget.Get().Should().BeNull();
            GameActions.GetSector(World, 8).SoundTarget.Get().Should().BeNull();
            GameActions.GetSector(World, 10).SoundTarget.Get().Should().BeNull();
            GameActions.GetSector(World, 11).SoundTarget.Get().Should().BeNull();
        }

        [Fact(DisplayName = "Sound from sector 4 with open door")]
        public void SoundDoorOpenSector4()
        {
            GameActions.GetSector(World, 6).Ceiling.SetZ(56);

            GameActions.SetEntityPosition(World, Player, new Vec2D(0, 480));
            World.NoiseAlert(Player, Player);

            GameActions.GetSector(World, 0).SoundTarget.Get().Should().Be(Player);
            GameActions.GetSector(World, 2).SoundTarget.Get().Should().Be(Player);
            GameActions.GetSector(World, 3).SoundTarget.Get().Should().Be(Player);
            GameActions.GetSector(World, 4).SoundTarget.Get().Should().Be(Player);
            GameActions.GetSector(World, 5).SoundTarget.Get().Should().Be(Player);
            GameActions.GetSector(World, 6).SoundTarget.Get().Should().Be(Player);

            GameActions.GetSector(World, 1).SoundTarget.Get().Should().BeNull();
            GameActions.GetSector(World, 7).SoundTarget.Get().Should().BeNull();
            GameActions.GetSector(World, 8).SoundTarget.Get().Should().BeNull();
            GameActions.GetSector(World, 10).SoundTarget.Get().Should().BeNull();
            GameActions.GetSector(World, 11).SoundTarget.Get().Should().BeNull();
        }

        [Fact(DisplayName = "Sound from sector 1")]
        public void SoundSector1()
        {
            GameActions.SetEntityPosition(World, Player, new Vec2D(-768, -288));
            World.NoiseAlert(Player, Player);

            // Sector 1 is completely closed off from the rest of the map
            GameActions.GetSector(World, 1).SoundTarget.Get().Should().Be(Player);

            GameActions.GetSector(World, 0).SoundTarget.Get().Should().BeNull();
            GameActions.GetSector(World, 2).SoundTarget.Get().Should().BeNull();
            GameActions.GetSector(World, 3).SoundTarget.Get().Should().BeNull();
            GameActions.GetSector(World, 4).SoundTarget.Get().Should().BeNull();
            GameActions.GetSector(World, 5).SoundTarget.Get().Should().BeNull();
            GameActions.GetSector(World, 6).SoundTarget.Get().Should().BeNull();
            GameActions.GetSector(World, 7).SoundTarget.Get().Should().BeNull();
            GameActions.GetSector(World, 8).SoundTarget.Get().Should().BeNull();
            GameActions.GetSector(World, 10).SoundTarget.Get().Should().BeNull();
            GameActions.GetSector(World, 11).SoundTarget.Get().Should().BeNull();
        }
    }
}
