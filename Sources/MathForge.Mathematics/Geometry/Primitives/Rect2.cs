using MathForge.LinearAlgebra.Vectors;

namespace MathForge.Geometry.Primitives;

public readonly record struct Rect2(Float2 Position, Size2 Size)
{
    public float MinX => Position.X;
    public float MinY => Position.Y;
    public float MaxX => Position.X + Size.Width;
    public float MaxY => Position.Y + Size.Height;
}
