﻿// THIS FILE WAS AUTO-GENERATED.
// CHANGES WILL NOT BE PROPAGATED.
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Helion.Geometry.Boxes;
using Helion.Geometry.Vectors;
using Helion.Util.Extensions;

namespace Helion.Geometry.Segments
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct Seg2D : IEquatable<Seg2D>
    {
        public Vec2D Start;
        public Vec2D End;

        public Vec2D Delta;
        public Box2D Box;
        public double Length => Start.Distance(End);
        public bool IsAxisAligned => Start.X.ApproxEquals(End.X) || Start.Y.ApproxEquals(End.Y);
        public IEnumerable<Vec2D> Vertices => GetVertices();

        public Seg2D(Vec2D start, Vec2D end)
        {
            Start = start;
            End = end;
            Delta = End - Start;
            Box = new((Start.X.Min(End.X), Start.Y.Min(End.Y)), (Start.X.Max(End.X), Start.Y.Max(End.Y)));
        }
        public Seg2D(Vec2D start, Vector2D end)
        {
            Start = start;
            End = end.Struct;
            Delta = End - Start;
            Box = new((Start.X.Min(End.X), Start.Y.Min(End.Y)), (Start.X.Max(End.X), Start.Y.Max(End.Y)));
        }
        public Seg2D(Vector2D start, Vec2D end)
        {
            Start = start.Struct;
            End = end;
            Delta = End - Start;
            Box = new((Start.X.Min(End.X), Start.Y.Min(End.Y)), (Start.X.Max(End.X), Start.Y.Max(End.Y)));
        }
        public Seg2D(Vector2D start, Vector2D end)
        {
            Start = start.Struct;
            End = end.Struct;
            Delta = End - Start;
            Box = new((Start.X.Min(End.X), Start.Y.Min(End.Y)), (Start.X.Max(End.X), Start.Y.Max(End.Y)));
        }

        public static implicit operator Seg2D(ValueTuple<Vec2D, Vec2D> tuple)
        {
            return new(tuple.Item1, tuple.Item2);
        }

        public void Deconstruct(out Vec2D start, out Vec2D end)
        {
            start = Start;
            end = End;
        }

        public Vec2D this[int index] => index == 0 ? Start : End;
        public Vec2D this[Endpoint endpoint] => endpoint == Endpoint.Start ? Start : End;

        public static Seg2D operator +(Seg2D self, Vec2D other) => new(self.Start + other, self.End + other);
        public static Seg2D operator +(Seg2D self, Vector2D other) => new(self.Start + other, self.End + other);
        public static Seg2D operator -(Seg2D self, Vec2D other) => new(self.Start - other, self.End - other);
        public static Seg2D operator -(Seg2D self, Vector2D other) => new(self.Start - other, self.End - other);
        public static bool operator ==(Seg2D self, Seg2D other) => self.Start == other.Start && self.End == other.End;
        public static bool operator !=(Seg2D self, Seg2D other) => !(self == other);

        public Vec2D Opposite(Endpoint endpoint) => endpoint == Endpoint.Start ? End : Start;
        public Seg2D WithStart(Vec2D start) => (start, End);
        public Seg2D WithStart(Vector2D start) => (start.Struct, End);
        public Seg2D WithEnd(Vec2D end) => (Start, end);
        public Seg2D WithEnd(Vector2D end) => (Start, end.Struct);
        public Vec2D FromTime(double t) => Start + (Delta * t);
        public bool SameDirection(Seg2D seg) => SameDirection(seg.Delta);
        public bool SameDirection(Segment2D seg) => SameDirection(seg.Delta);
        public bool SameDirection<T>(SegmentT2D<T> seg) where T : Vector2D => SameDirection(seg.Delta);
        public bool SameDirection(Vec2D delta)
        {
            return !Delta.X.DifferentSign(delta.X) && !Delta.Y.DifferentSign(delta.Y);
        }
        public bool SameDirection(Vector2D delta)
        {
            return !Delta.X.DifferentSign(delta.X) && !Delta.Y.DifferentSign(delta.Y);
        }
        public double PerpDot(in Vec2D point)
        {
            return (Delta.X * (point.Y - Start.Y)) - (Delta.Y * (point.X - Start.X));
        }
        public double PerpDot(in Vec3D point)
        {
            return (Delta.X * (point.Y - Start.Y)) - (Delta.Y * (point.X - Start.X));
        }
        public double PerpDot(Vector2D point)
        {
            return (Delta.X * (point.Y - Start.Y)) - (Delta.Y * (point.X - Start.X));
        }
        public double PerpDot(double x, double y)
        {
            return (Delta.X * (y - Start.Y)) - (Delta.Y * (x - Start.X));
        }
        public double PerpDot(Vector3D point) => PerpDot(point.XY);
        public bool OnRight(Vec2D point) => PerpDot(point) <= 0;
        public bool OnRight(Vector2D point) => PerpDot(point) <= 0;
        public bool OnRight(Vec3D point) => PerpDot(point) <= 0;
        public bool OnRight(Vector3D point) => PerpDot(point.XY) <= 0;
        public bool OnRight(Seg2D seg) => OnRight(seg.Start) && OnRight(seg.End);
        public bool OnRight(Segment2D seg) => OnRight(seg.Start) && OnRight(seg.End);
        public bool OnRight<T>(SegmentT2D<T> seg) where T : Vector2D => OnRight(seg.Start) && OnRight(seg.End);
        public bool DifferentSides(Vec2D first, Vec2D second) => OnRight(first) != OnRight(second);
        public bool DifferentSides(Vector2D first, Vector2D second) => OnRight(first) != OnRight(second);
        public bool DifferentSides(Seg2D seg) => OnRight(seg.Start) != OnRight(seg.End);
        public bool DifferentSides(Segment2D seg) => OnRight(seg.Start) != OnRight(seg.End);
        public bool DifferentSides<T>(SegmentT2D<T> seg) where T : Vector2D => OnRight(seg.Start) != OnRight(seg.End);
        public Rotation ToSide(Vec2D point, double epsilon = 0.000001)
        {
            double value = PerpDot(point);
            bool approxZero = value.ApproxZero(epsilon);
            return approxZero ? Rotation.On : (value < 0 ? Rotation.Right : Rotation.Left);
        }
        public Rotation ToSide(Vector2D point, double epsilon = 0.000001)
        {
            double value = PerpDot(point);
            bool approxZero = value.ApproxZero(epsilon);
            return approxZero ? Rotation.On : (value < 0 ? Rotation.Right : Rotation.Left);
        }
        public double ToTime(Vec2D point)
        {
            if (Start.X.ApproxEquals(End.X))
                return (point.Y - Start.Y) / (End.Y - Start.Y);
            return (point.X - Start.X) / (End.X - Start.X);
        }
        public double ToTime(Vector2D point)
        {
            if (Start.X.ApproxEquals(End.X))
                return (point.Y - Start.Y) / (End.Y - Start.Y);
            return (point.X - Start.X) / (End.X - Start.X);
        }
        public bool Parallel(Seg2D seg, double epsilon = 0.000001)
        {
            return (Delta.Y * seg.Delta.X).ApproxEquals(Delta.X * seg.Delta.Y, epsilon);
        }
        public bool Parallel(Segment2D seg, double epsilon = 0.000001)
        {
            return (Delta.Y * seg.Delta.X).ApproxEquals(Delta.X * seg.Delta.Y, epsilon);
        }
        public bool Parallel<T>(SegmentT2D<T> seg, double epsilon = 0.000001) where T : Vector2D
        {
            return (Delta.Y * seg.Delta.X).ApproxEquals(Delta.X * seg.Delta.Y, epsilon);
        }
        public bool Collinear(Seg2D seg)
        {
            return CollinearHelper(seg.Start.X, seg.Start.Y, Start.X, Start.Y, End.X, End.Y) &&
                   CollinearHelper(seg.End.X, seg.End.Y, Start.X, Start.Y, End.X, End.Y);
        }
        public bool Collinear(Segment2D seg)
        {
            return CollinearHelper(seg.Start.X, seg.Start.Y, Start.X, Start.Y, End.X, End.Y) &&
                   CollinearHelper(seg.End.X, seg.End.Y, Start.X, Start.Y, End.X, End.Y);
        }
        public bool Collinear<T>(SegmentT2D<T> seg) where T : Vector2D
        {
            return CollinearHelper(seg.Start.X, seg.Start.Y, Start.X, Start.Y, End.X, End.Y) &&
                   CollinearHelper(seg.End.X, seg.End.Y, Start.X, Start.Y, End.X, End.Y);
        }
        public bool Intersects(Seg2D other) => Intersection(other, out double t) && (t >= 0 && t <= 1);
        public bool Intersects(Segment2D other) => Intersection(other, out double t) && (t >= 0 && t <= 1);
        public bool Intersects<T>(SegmentT2D<T> other) where T : Vector2D => Intersection(other, out double t) && (t >= 0 && t <= 1);

        public readonly bool Intersection(in Seg2D seg, out double t)
        {
            double startX = Start.X;
            double startY = Start.Y;
            double endX = End.X;
            double endY = End.Y;
            double segStartX = seg.Start.X;
            double segStartY = seg.Start.Y;
            double segEndX = seg.End.X;
            double segEndY = seg.End.Y;
            double areaStart = ((startX - segEndX) * (endY - segEndY)) - ((startY - segEndY) * (endX - segEndX));
            double areaEnd = ((startX - segStartX) * (endY - segStartY)) - ((startY - segStartY) * (endX - segStartX));

            if (areaStart * areaEnd < 0)
            {
                double areaThisStart = ((segStartX - startX) * (segEndY - startY)) - ((segStartY - startY) * (segEndX - startX));
                double areaThisEnd = ((segStartX - endX) * (segEndY - endY)) - ((segStartY - endY) * (segEndX - endX));

                if (areaThisStart * areaThisEnd < 0)
                {
                    t = areaThisStart / (areaThisStart - areaThisEnd);
                    return t >= 0 && t <= 1;
                }
            }

            t = default;
            return false;
        }

        public readonly bool Intersection(double segStartX, double segStartY, double segEndX, double segEndY, out double t)
        {
            double startX = Start.X;
            double startY = Start.Y;
            double endX = End.X;
            double endY = End.Y;
            double areaStart = ((startX - segEndX) * (endY - segEndY)) - ((startY - segEndY) * (endX - segEndX));
            double areaEnd = ((startX - segStartX) * (endY - segStartY)) - ((startY - segStartY) * (endX - segStartX));

            if (areaStart * areaEnd < 0)
            {
                double areaThisStart = ((segStartX - startX) * (segEndY - startY)) - ((segStartY - startY) * (segEndX - startX));
                double areaThisEnd = ((segStartX - endX) * (segEndY - endY)) - ((segStartY - endY) * (segEndX - endX));

                if (areaThisStart * areaThisEnd < 0)
                {
                    t = areaThisStart / (areaThisStart - areaThisEnd);
                    return t >= 0 && t <= 1;
                }
            }

            t = default;
            return false;
        }

        public bool Intersection(Segment2D seg, out double t)
        {
            double areaStart = DoubleTriArea(Start.X, Start.Y, End.X, End.Y, seg.End.X, seg.End.Y);
            double areaEnd = DoubleTriArea(Start.X, Start.Y, End.X, End.Y, seg.Start.X, seg.Start.Y);

            if (areaStart.DifferentSign(areaEnd))
            {
                double areaThisStart = DoubleTriArea(seg.Start.X, seg.Start.Y, seg.End.X, seg.End.Y, Start.X, Start.Y);
                double areaThisEnd = DoubleTriArea(seg.Start.X, seg.Start.Y, seg.End.X, seg.End.Y, End.X, End.Y);

                if (areaStart.DifferentSign(areaEnd))
                {
                    t = areaThisStart / (areaThisStart - areaThisEnd);
                    return t >= 0 && t <= 1;
                }
            }

            t = default;
            return false;
        }
        public bool Intersection<T>(SegmentT2D<T> seg, out double t) where T : Vector2D
        {
            double areaStart = DoubleTriArea(Start.X, Start.Y, End.X, End.Y, seg.End.X, seg.End.Y);
            double areaEnd = DoubleTriArea(Start.X, Start.Y, End.X, End.Y, seg.Start.X, seg.Start.Y);

            if (areaStart.DifferentSign(areaEnd))
            {
                double areaThisStart = DoubleTriArea(seg.Start.X, seg.Start.Y, seg.End.X, seg.End.Y, Start.X, Start.Y);
                double areaThisEnd = DoubleTriArea(seg.Start.X, seg.Start.Y, seg.End.X, seg.End.Y, End.X, End.Y);

                if (areaStart.DifferentSign(areaEnd))
                {
                    t = areaThisStart / (areaThisStart - areaThisEnd);
                    return t >= 0 && t <= 1;
                }
            }

            t = default;
            return false;
        }
        public bool IntersectionAsLine(Seg2D seg, out double tThis)
        {
            double determinant = (-seg.Delta.X * Delta.Y) + (Delta.X * seg.Delta.Y);
            if (determinant.ApproxZero())
            {
                tThis = default;
                return false;
            }

            Vec2D startDelta = Start - seg.Start;
            tThis = ((seg.Delta.X * startDelta.Y) - (seg.Delta.Y * startDelta.X)) / determinant;
            return true;
        }
        public bool IntersectionAsLine(Segment2D seg, out double tThis)
        {
            double determinant = (-seg.Delta.X * Delta.Y) + (Delta.X * seg.Delta.Y);
            if (determinant.ApproxZero())
            {
                tThis = default;
                return false;
            }

            Vec2D startDelta = Start - seg.Start;
            tThis = ((seg.Delta.X * startDelta.Y) - (seg.Delta.Y * startDelta.X)) / determinant;
            return true;
        }
        public bool IntersectionAsLine<T>(SegmentT2D<T> seg, out double tThis) where T : Vector2D
        {
            double determinant = (-seg.Delta.X * Delta.Y) + (Delta.X * seg.Delta.Y);
            if (determinant.ApproxZero())
            {
                tThis = default;
                return false;
            }

            Vec2D startDelta = Start - seg.Start;
            tThis = ((seg.Delta.X * startDelta.Y) - (seg.Delta.Y * startDelta.X)) / determinant;
            return true;
        }
        public bool IntersectionAsLine(Seg2D seg, out double tThis, out double tOther)
        {
            double determinant = (-seg.Delta.X * Delta.Y) + (Delta.X * seg.Delta.Y);
            if (determinant.ApproxZero())
            {
                tThis = default;
                tOther = default;
                return false;
            }

            Vec2D startDelta = Start - seg.Start;
            double inverseDeterminant = 1.0f / determinant;
            tThis = ((seg.Delta.X * startDelta.Y) - (seg.Delta.Y * startDelta.X)) * inverseDeterminant;
            tOther = ((-Delta.Y * startDelta.X) + (Delta.X * startDelta.Y)) * inverseDeterminant;
            return true;
        }
        public bool IntersectionAsLine(Segment2D seg, out double tThis, out double tOther)
        {
            double determinant = (-seg.Delta.X * Delta.Y) + (Delta.X * seg.Delta.Y);
            if (determinant.ApproxZero())
            {
                tThis = default;
                tOther = default;
                return false;
            }

            Vec2D startDelta = Start - seg.Start;
            double inverseDeterminant = 1.0f / determinant;
            tThis = ((seg.Delta.X * startDelta.Y) - (seg.Delta.Y * startDelta.X)) * inverseDeterminant;
            tOther = ((-Delta.Y * startDelta.X) + (Delta.X * startDelta.Y)) * inverseDeterminant;
            return true;
        }
        public bool IntersectionAsLine<T>(SegmentT2D<T> seg, out double tThis, out double tOther) where T : Vector2D
        {
            double determinant = (-seg.Delta.X * Delta.Y) + (Delta.X * seg.Delta.Y);
            if (determinant.ApproxZero())
            {
                tThis = default;
                tOther = default;
                return false;
            }

            Vec2D startDelta = Start - seg.Start;
            double inverseDeterminant = 1.0f / determinant;
            tThis = ((seg.Delta.X * startDelta.Y) - (seg.Delta.Y * startDelta.X)) * inverseDeterminant;
            tOther = ((-Delta.Y * startDelta.X) + (Delta.X * startDelta.Y)) * inverseDeterminant;
            return true;
        }
        public Vec2D ClosestPoint(Vec2D point)
        {
            Vec2D pointToStartDelta = Start - point;
            double t = -Delta.Dot(pointToStartDelta) / Delta.Dot(Delta);

            if (t <= 0)
                return Start;
            if (t >= 1)
                return End;
            return FromTime(t);
        }
        public Vec2D ClosestPoint(Vector2D point)
        {
            Vec2D pointToStartDelta = Start - point;
            double t = -Delta.Dot(pointToStartDelta) / Delta.Dot(Delta);

            if (t <= 0)
                return Start;
            if (t >= 1)
                return End;
            return FromTime(t);
        }
        public bool Intersects(double boxMinX, double boxMinY, double boxMaxX, double boxMaxY)
        {
            if (Box.Min.X >= boxMaxX || Box.Max.X <= boxMinX || Box.Min.Y >= boxMaxY || Box.Max.Y <= boxMinY)
                return false;

            double startX = Start.X;
            double startY = Start.Y;
            double endX = End.X;
            double endY = End.Y;

            Vec2D delta = Delta;
            if ((startX < endX) ^ (startY < endY))
            {
                return ((delta.X * (boxMinY - startY)) - (delta.Y * (boxMinX - startX))) < 0 !=
                    ((delta.X * (boxMaxY - startY)) - (delta.Y * (boxMaxX - startX))) < 0;
            }
            else
            {
                return ((delta.X * (boxMaxY - startY)) - (delta.Y * (boxMinX - startX))) < 0 !=
                    ((delta.X * (boxMinY - startY)) - (delta.Y * (boxMaxX - startX))) < 0;
            }
        }
        public bool Intersects(in Box2D box)
        {
            if (Box.Min.X >= box.Max.X || Box.Max.X <= box.Min.X || Box.Min.Y >= box.Max.Y || Box.Max.Y <= box.Min.Y)
                return false;

            double startX = Start.X;
            double startY = Start.Y;
            double endX = End.X;
            double endY = End.Y;

            Vec2D delta = Delta;
            if ((startX < endX) ^ (startY < endY))
            {
                return ((delta.X * (box.Min.Y - startY)) - (delta.Y * (box.Min.X - startX))) < 0 !=
                    ((delta.X * (box.Max.Y - startY)) - (delta.Y * (box.Max.X - startX))) < 0;
            }
            else
            {
                return ((delta.X * (box.Max.Y - startY)) - (delta.Y * (box.Min.X - startX))) < 0 !=
                    ((delta.X * (box.Min.Y - startY)) - (delta.Y * (box.Max.X - startX))) < 0;
            }
        }
        public bool Intersects(BoundingBox2D box)
        {
            if (!box.Overlaps(Box))
                return false;
            if (Start.X.ApproxEquals(End.X))
                return box.Min.X < Start.X && Start.X < box.Max.X;
            if (Start.Y.ApproxEquals(End.Y))
                return box.Min.Y < Start.Y && Start.Y < box.Max.Y;
            return ((Start.X < End.X) ^ (Start.Y < End.Y)) ?
                DifferentSides(box.BottomLeft, box.TopRight) :
                DifferentSides(box.TopLeft, box.BottomRight);
        }

        private static bool CollinearHelper(double aX, double aY, double bX, double bY, double cX, double cY)
        {
            return ((aX * (bY - cY)) + (bX * (cY - aY)) + (cX * (aY - bY))).ApproxZero();
        }
        private static double DoubleTriArea(double aX, double aY, double bX, double bY, double cX, double cY)
        {
            return ((aX - cX) * (bY - cY)) - ((aY - cY) * (bX - cX));
        }
        public readonly override string ToString() => $"({Start}), ({End})";
        public readonly override int GetHashCode() => HashCode.Combine(Start.GetHashCode(), End.GetHashCode());
        public readonly bool Equals(Seg2D other) => Start.X == other.Start.X && Start.Y == other.Start.Y && End.X == other.End.X && End.Y == other.End.Y;
        public readonly override bool Equals(object? obj) => obj is Seg2D && Equals((Seg2D)obj);

        private readonly IEnumerable<Vec2D> GetVertices()
        {
            yield return Start;
            yield return End;
        }
    }
}
