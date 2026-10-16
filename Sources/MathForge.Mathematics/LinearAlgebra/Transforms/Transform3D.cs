using MathForge.LinearAlgebra.Vectors;

namespace MathForge.LinearAlgebra.Matrices;

public struct Transform3D
{
	public Float3 Position { get; set; }
	public Quaternion Rotation { get; set; }
	public Float3 Scale { get; set; }
}