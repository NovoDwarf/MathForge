using MathForge.LinearAlgebra.Vectors;

namespace MathForge.LinearAlgebra.Transforms;

public struct Transform2D
{
	public Float2 Position { get; set; }
	public float Rotation { get; set; }
	public Float2 Scale { get; set; }
}