using MathForge.Vectors.Integer;

namespace MathForge.Neighborhoods;

public interface INeighborhood2D
{
	public IReadOnlyList<Int2> Offsets2D { get; }
}