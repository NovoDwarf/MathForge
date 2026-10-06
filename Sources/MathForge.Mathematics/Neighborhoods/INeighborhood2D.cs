using MathForge.Vectors.Integer;

namespace MathForge.Neighborhoods;

public interface INeighborhood2D
{
	public IReadOnlyList<Int2> Offsets2D { get; }

	public IEnumerable<Int2> GetNeighbors(int x, int y);
	
	public IEnumerable<Int2> GetNeighbors(Int2 coordinate);
}