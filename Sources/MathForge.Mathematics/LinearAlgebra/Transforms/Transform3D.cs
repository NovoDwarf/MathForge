using MathForge.LinearAlgebra.Quaternions;
using MathForge.LinearAlgebra.Vectors;

namespace MathForge.LinearAlgebra.Transforms;

public struct Transform3D
{
	public Float3 Position { get; set; }
	public Quaternion Rotation { get; set; }
	public Float3 Scale { get; set; }
}