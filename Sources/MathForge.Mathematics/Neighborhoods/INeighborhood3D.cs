using MathForge.Vectors.Integer;

namespace MathForge.Neighborhoods;

public interface INeighborhood3D
{
	public IReadOnlyList<Int3> Offsets3D { get; }
}