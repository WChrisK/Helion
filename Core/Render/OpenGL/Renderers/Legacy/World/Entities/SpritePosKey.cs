using Helion.Geometry.Vectors;
using System;

namespace Helion.Render.OpenGL.Renderers.Legacy.World.Entities;

public readonly struct SpritePosKey(Vec2D pos, int sprite) : IEquatable<SpritePosKey>
{
    public readonly Vec2D Pos = pos;
    public readonly int Sprite = sprite;

    public override int GetHashCode() => HashCode.Combine(Pos.X, Pos.Y, Sprite);
    public readonly bool Equals(SpritePosKey other) => Sprite == other.Sprite && other.Pos.X == Pos.X && other.Pos.Y == Pos.Y;
    public readonly override bool Equals(object? obj) => obj is not null && obj is SpritePosKey key && Equals(key);
    public static bool operator ==(SpritePosKey left, SpritePosKey right) => left.Equals(right);
    public static bool operator !=(SpritePosKey left, SpritePosKey right) => !(left == right);
}
