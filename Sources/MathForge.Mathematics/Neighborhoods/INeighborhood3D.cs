using MathForge.Vectors.Integer;

namespace MathForge.Neighborhoods;

public interface INeighborhood3D
{
	public IReadOnlyList<Int3> Offsets3D { get; }
	
	public IEnumerable<Int3> GetNeighbors(int x, int y, int z);
	
	public IEnumerable<Int3> GetNeighbors(Int3 coordinate);
}