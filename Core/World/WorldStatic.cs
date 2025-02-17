﻿using Helion.Geometry.Grids;
using Helion.Util;
using Helion.Util.Container;
using Helion.Util.RandomGenerators;
using Helion.World.Blockmap;
using Helion.World.Entities;
using Helion.World.Entities.Definition;
using Helion.World.Entities.Definition.States;
using Helion.World.Physics.Blockmap;
using Helion.World.Sound;
using System.Collections.Generic;

namespace Helion.World;

public static class WorldStatic
{
    public static DynamicArray<BlockmapIntersect> Intersections = new(1024);
    public static IWorld World = null!;
    public static IRandom Random = null!;
    public static DataCache DataCache = null!;
    public static int CheckCounter;
    public static int SlowTickDistance;
    public static int RespawnTicks;
    public static int ClosetLookFrameIndex;
    public static int ClosetChaseFrameIndex;
    public static short SlowTickChaseFailureSkipCount;
    public static short SlowTickChaseMultiplier;
    public static short SlowTickLookMultiplier;
    public static short SlowTickTracerMultiplier;
    public static bool SlowTickEnabled;
    public static bool IsFastMonsters;
    public static bool IsSlowMonsters;
    public static bool InfinitelyTallThings;
    public static bool MissileClip;
    public static bool AllowItemDropoff;
    public static bool NoTossDrops;
    public static bool VanillaMovementPhysics;
    public static bool Dehacked;
    public static bool Mbf21;
    public static bool Doom2ProjectileWalkTriggers;
    public static bool OriginalExplosion;
    public static bool FinalDoomTeleport;
    public static bool VanillaSectorSound;
    public static EntityManager EntityManager = null!;
    public static WorldSoundManager SoundManager = null!;
    public static List<EntityFrame> Frames = null!;
    public static int[] CheckedLines = [];

    public static EntityDefinition DoomImpBall = EntityDefinition.Default;
    public static EntityDefinition ArachnotronPlasma = EntityDefinition.Default;
    public static EntityDefinition Rocket = EntityDefinition.Default;
    public static EntityDefinition FatShot = EntityDefinition.Default;
    public static EntityDefinition CacodemonBall = EntityDefinition.Default;
    public static EntityDefinition RevenantTracer = EntityDefinition.Default;
    public static EntityDefinition BaronBall = EntityDefinition.Default;
    public static EntityDefinition SpawnShot = EntityDefinition.Default;
    public static EntityDefinition BFGBall = EntityDefinition.Default;
    public static EntityDefinition PlasmaBall = EntityDefinition.Default;

    public static EntityDefinition WeaponBfg = EntityDefinition.Default;
}
