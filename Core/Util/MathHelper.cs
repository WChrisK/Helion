using System;
using System.Runtime.CompilerServices;
using Helion.Geometry;
using Helion.Geometry.Vectors;
using Helion.World.Geometry.Lines;

namespace Helion.Util;

/// <summary>
/// A collection of helpers for mathematical functions and constants.
/// </summary>
public static class MathHelper
{
    public const double Pi = Math.PI;
    public const double QuarterPi = Math.PI / 4;
    public const double HalfPi = Math.PI / 2;
    public const double TwoPi = 2 * Math.PI;

    /// <summary>
    /// The value that can be multiplied with to change degrees to radians.
    /// </summary>
    public const double DegreesToRadiansFactor = TwoPi / 360.0;

    /// <summary>
    /// Checks if the value is equal to zero.
    /// </summary>
    /// <param name="fixedPoint">The value to check.</param>
    /// <returns>True if it does, false if not.</returns>
    public static bool IsZero(Fixed fixedPoint) => fixedPoint.Bits == 0;

    /// <summary>
    /// Checks if the value is equal to zero within some epsilon.
    /// </summary>
    /// <param name="fixedPoint">The value to check.</param>
    /// <param name="epsilon">The epsilon value.</param>
    /// <returns>True if it's within some epsilon, false otherwise.
    /// </returns>
    public static bool IsZero(Fixed fixedPoint, Fixed epsilon) => fixedPoint.Abs() < epsilon;

    /// <summary>
    /// Checks if the value is equal to zero within some epsilon.
    /// </summary>
    /// <param name="f">The value to check.</param>
    /// <param name="epsilon">The epsilon value.</param>
    /// <returns>True if it's within some epsilon, false otherwise.
    /// </returns>
    public static bool IsZero(float f, float epsilon = 0.00001f) => Math.Abs(f) < epsilon;

    /// <summary>
    /// Checks if the value is equal to zero within some epsilon.
    /// </summary>
    /// <param name="d">The value to check.</param>
    /// <param name="epsilon">The epsilon value.</param>
    /// <returns>True if it's within some epsilon, false otherwise.
    /// </returns>
    public static bool IsZero(double d, double epsilon = 0.00001) => Math.Abs(d) < epsilon;

    /// <summary>
    /// Checks if the values are equal to one another.
    /// </summary>
    /// <param name="first">The first value to compare.</param>
    /// <param name="second">The second value to compare.</param>
    /// <returns>True if they're equal, false if not.</returns>
    public static bool AreEqual(Fixed first, Fixed second) => first == second;

    /// <summary>
    /// Checks if the values are equal to one another within some epsilon.
    /// </summary>
    /// <param name="first">The first value to compare.</param>
    /// <param name="second">The second value to compare.</param>
    /// <param name="epsilon">The epsilon to check again.</param>
    /// <returns>True if they're equal within some epsilon, false if not.
    /// </returns>
    public static bool AreEqual(Fixed first, Fixed second, Fixed epsilon) => (first - second).Abs() < epsilon;

    /// <summary>
    /// Checks if the values are equal to one another within some epsilon.
    /// </summary>
    /// <param name="first">The first value to compare.</param>
    /// <param name="second">The second value to compare.</param>
    /// <param name="epsilon">The epsilon to check again.</param>
    /// <returns>True if they're equal within some epsilon, false if not.
    /// </returns>
    public static bool AreEqual(float first, float second, float epsilon = 0.00001f) => Math.Abs(first - second) < epsilon;

    /// <summary>
    /// Checks if the values are equal to one another within some epsilon.
    /// </summary>
    /// <param name="first">The first value to compare.</param>
    /// <param name="second">The second value to compare.</param>
    /// <param name="epsilon">The epsilon to check again.</param>
    /// <returns>True if they're equal within some epsilon, false if not.
    /// </returns>
    public static bool AreEqual(double first, double second, double epsilon = 0.000001) => Math.Abs(first - second) < epsilon;

    /// <summary>
    /// Checks if the signs are different.
    /// </summary>
    /// <param name="first">The first value to check.</param>
    /// <param name="second">The second value to check.</param>
    /// <returns>True if the signs are different, false if not.</returns>
    public static bool DifferentSign(int first, int second) => (first ^ second) < 0;

    /// <summary>
    /// Checks if the signs are different.
    /// </summary>
    /// <param name="first">The first value to check.</param>
    /// <param name="second">The second value to check.</param>
    /// <returns>True if the signs are different, false if not.</returns>
    public static bool DifferentSign(Fixed first, Fixed second) => (first.Bits ^ second.Bits) < 0;

    /// <summary>
    /// Checks if the signs are different.
    /// </summary>
    /// <param name="first">The first value to check.</param>
    /// <param name="second">The second value to check.</param>
    /// <returns>True if the signs are different, false if not.</returns>
    public static bool DifferentSign(float first, float second) => (first * second) < 0;

    /// <summary>
    /// Checks if the signs are different.
    /// </summary>
    /// <param name="first">The first value to check.</param>
    /// <param name="second">The second value to check.</param>
    /// <returns>True if the signs are different, false if not.</returns>
    public static bool DifferentSign(double first, double second) => (first * second) < 0;

    /// <summary>
    /// Checks if the value is in the [0.0, 1.0] range.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <returns>True if it is, false if not.</returns>
    public static bool InNormalRange(float value) => value >= 0 && value <= 1;

    /// <summary>
    /// Checks if the value is in the [0.0, 1.0] range.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <returns>True if it is, false if not.</returns>
    public static bool InNormalRange(double value) => value >= 0 && value <= 1;

    /// <summary>
    /// Gets the minimum value of two fixed point numbers.
    /// </summary>
    /// <param name="first">The first number.</param>
    /// <param name="second">The second number.</param>
    /// <returns>The minimum value of both.</returns>
    public static Fixed Min(Fixed first, Fixed second) => first.Bits < second.Bits ? first : second;

    /// <summary>
    /// Gets the maximum value of two fixed point numbers.
    /// </summary>
    /// <param name="first">The first number.</param>
    /// <param name="second">The second number.</param>
    /// <returns>The maximum value of both.</returns>
    public static Fixed Max(Fixed first, Fixed second) => first.Bits < second.Bits ? second : first;

    /// <summary>
    /// Gets the max of a list of integers. Avoids any allocations.
    /// </summary>
    /// <param name="ints">The integers to get the maximum value from.
    /// </param>
    /// <returns>Returns the max, or -MaxInt if the list is empty.
    /// </returns>
    public static int Max(params int[] ints)
    {
        int max = int.MinValue;
        for (int i = 0; i < ints.Length; i++)
            max = Math.Max(max, ints[i]);
        return max;
    }

    /// <summary>
    /// Converts the degrees into radians.
    /// </summary>
    /// <param name="degree">The degree.</param>
    /// <returns>The radian value.</returns>
    public static double ToRadians(double degree) => degree * DegreesToRadiansFactor;

    /// <summary>
    /// Clamps the value between two ranges inclusively.
    /// </summary>
    /// <param name="value">The value to clamp.</param>
    /// <param name="low">The lower bound (inclusive).</param>
    /// <param name="high">The upper bound (inclusive).</param>
    /// <returns>The clamped value.</returns>
    public static byte Clamp(byte value, byte low, byte high)
    {
        return value < low ? low : (value > high ? high : value);
    }

    /// <summary>
    /// Clamps the value between two ranges inclusively.
    /// </summary>
    /// <param name="value">The value to clamp.</param>
    /// <param name="low">The lower bound (inclusive).</param>
    /// <param name="high">The upper bound (inclusive).</param>
    /// <returns>The clamped value.</returns>
    public static short Clamp(short value, short low, short high)
    {
        return value < low ? low : (value > high ? high : value);
    }

    /// <summary>
    /// Clamps the value between two ranges inclusively.
    /// </summary>
    /// <param name="value">The value to clamp.</param>
    /// <param name="low">The lower bound (inclusive).</param>
    /// <param name="high">The upper bound (inclusive).</param>
    /// <returns>The clamped value.</returns>
    public static ushort Clamp(ushort value, ushort low, ushort high)
    {
        return value < low ? low : (value > high ? high : value);
    }

    /// <summary>
    /// Clamps the value between two ranges inclusively.
    /// </summary>
    /// <param name="value">The value to clamp.</param>
    /// <param name="low">The lower bound (inclusive).</param>
    /// <param name="high">The upper bound (inclusive).</param>
    /// <returns>The clamped value.</returns>
    public static int Clamp(int value, int low, int high)
    {
        return value < low ? low : (value > high ? high : value);
    }

    /// <summary>
    /// Clamps the value between two ranges inclusively.
    /// </summary>
    /// <param name="value">The value to clamp.</param>
    /// <param name="low">The lower bound (inclusive).</param>
    /// <param name="high">The upper bound (inclusive).</param>
    /// <returns>The clamped value.</returns>
    public static uint Clamp(uint value, uint low, uint high)
    {
        return value < low ? low : (value > high ? high : value);
    }

    /// <summary>
    /// Clamps the value between two ranges inclusively.
    /// </summary>
    /// <param name="value">The value to clamp.</param>
    /// <param name="low">The lower bound (inclusive).</param>
    /// <param name="high">The upper bound (inclusive).</param>
    /// <returns>The clamped value.</returns>
    public static long Clamp(long value, long low, long high)
    {
        return value < low ? low : (value > high ? high : value);
    }

    /// <summary>
    /// Clamps the value between two ranges inclusively.
    /// </summary>
    /// <param name="value">The value to clamp.</param>
    /// <param name="low">The lower bound (inclusive).</param>
    /// <param name="high">The upper bound (inclusive).</param>
    /// <returns>The clamped value.</returns>
    public static ulong Clamp(ulong value, ulong low, ulong high)
    {
        return value < low ? low : (value > high ? high : value);
    }

    /// <summary>
    /// Clamps the value between two ranges inclusively. This does not have
    /// defined behavior for infinities or NaN's.
    /// </summary>
    /// <param name="value">The value to clamp.</param>
    /// <param name="low">The lower bound (inclusive).</param>
    /// <param name="high">The upper bound (inclusive).</param>
    /// <returns>The clamped value.</returns>
    public static float Clamp(float value, float low, float high)
    {
        return value < low ? low : (value > high ? high : value);
    }

    /// <summary>
    /// Clamps the value between two ranges inclusively. This does not have
    /// defined behavior for infinities or NaN's.
    /// </summary>
    /// <param name="value">The value to clamp.</param>
    /// <param name="low">The lower bound (inclusive).</param>
    /// <param name="high">The upper bound (inclusive).</param>
    /// <returns>The clamped value.</returns>
    public static double Clamp(double value, double low, double high)
    {
        return value < low ? low : (value > high ? high : value);
    }

    /// <summary>
    /// Takes two inputs and returns the pair in the min and max form.
    /// </summary>
    /// <param name="first">The first value.</param>
    /// <param name="second">The second value.</param>
    /// <returns>A pair that is ordered.</returns>
    public static (int min, int max) MinMax(int first, int second)
    {
        return first < second ? (first, second) : (second, first);
    }

    /// <summary>
    /// Takes two inputs and returns the pair in the min and max form.
    /// </summary>
    /// <param name="first">The first value.</param>
    /// <param name="second">The second value.</param>
    /// <returns>A pair that is ordered.</returns>
    public static (uint min, uint max) MinMax(uint first, uint second)
    {
        return first < second ? (first, second) : (second, first);
    }

    /// <summary>
    /// Converts double value to fixed point integer 16.16.
    /// </summary>
    /// <param name="value">Double value to convert.</param>
    /// <returns>Fixed point integer in 16.16 format.</returns>
    public static int ToFixed(double value)
    {
        return (int)(value * (1 << 16));
    }

    /// <summary>
    /// Converts fixed point integer 16.16 to double.
    /// </summary>
    /// <param name="value">Fixed point value to convert.</param>
    /// <returns>Converted double.</returns>
    public static double FromFixed(int value)
    {
        return value / 65536.0;
    }

    /// <summary>
    /// Takes a radian angle and ensures the angle is between 0 and 2pi.
    /// E.g. -6 degrees would return 354 degrees.
    /// </summary>
    /// <param name="angleRadians">The radian angle.</param>
    /// <returns>Angle between 0 and 2pi.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double GetPositiveAngle(double angleRadians)
    {
        angleRadians = Math.IEEERemainder(angleRadians, TwoPi);
        if (angleRadians < 0)
            return TwoPi + angleRadians;
        return angleRadians;
    }

    // Returns radian angle in the range of -2pi and 2pi
    public static double GetNormalAngle(double angleRadians) =>
        angleRadians % TwoPi;

    public static double ApproximateDistance(int dx, int dy)
    {
        dx = Math.Abs(dx);
        dy = Math.Abs(dy);
        if (dx < dy)
            return dx + dy - (dx / 2);
        return dx + dy - (dy / 2);
    }

    /// <summary>
    /// Gets the highest bit index. If there are no bits set (as in n = 0)
    /// then this returns -1. For example, 0x00000010 would return 4.
    /// </summary>
    /// <param name="n">The number to evaluate.</param>
    /// <returns>The index of the highest bit.</returns>
    public static int HighestBitIndex(int n)
    {
        for (int i = 31; i >= 0; i--)
            if ((n & (1 << i)) != 0)
                return i;
        return -1;
    }

    /// <summary>
    /// Checks if a number is a power of two. Will return false on zero or
    /// negative numbers.
    /// </summary>
    /// <param name="n">The bits to check</param>
    /// <returns>True if it's a power of two (one bit set), false otherwise.
    /// </returns>
    public static bool IsPow2(int n)
    {
        // Minus one to make the lower bits all 1, then xor it to get a mask for
        // the lowest bits, then and by the complement to find out if there's any
        // bits that are set higher than the rightmost bit we found. If so, then
        // it's not a power of two. Classic bit twiddling trick.
        return n > 0 && (n & ~(n ^ (n - 1))) == 0;
    }

    public static Vec2D BounceVelocity(Vec2D velocity, Line? line)
    {
        double velocityAngle = Math.Atan2(velocity.X, velocity.Y);
        if (line == null)
            return velocity.Rotate(Pi);

        double newAngle = 2 * line.Segment.Start.Angle(line.Segment.End) - velocityAngle;
        if (GetPositiveAngle(newAngle) == GetPositiveAngle(velocityAngle))
            newAngle += Pi;
        return velocity.Rotate(newAngle - velocityAngle);
    }
}
