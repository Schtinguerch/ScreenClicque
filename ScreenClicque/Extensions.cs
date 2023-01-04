using System;
using System.Drawing;

namespace ScreenClicque;

public static class Extensions
{
    public static Point Center(this Rectangle rectangle)
    {
        var midWidth = rectangle.Width / 2;
        var midHeight = rectangle.Height / 2;

        return new Point(rectangle.X + midWidth, rectangle.Y + midHeight);
    }
    
    public static Point WithOffset(this Point point, int offset)
    {
        return new Point(point.X + offset, point.Y + offset);
    }
    
    public static double Range(this Point a, Point b)
    {
        return Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y- a.Y, 2));
    }
}