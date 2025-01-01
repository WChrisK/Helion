namespace Helion.Models;

public struct Vector2D
{
    public Vector2D()
    {

    }

    public Vector2D(double x, double y)
    {
        X = x;
        Y = y;
    }

    public double X { get; set; }
    public double Y { get; set; }
}