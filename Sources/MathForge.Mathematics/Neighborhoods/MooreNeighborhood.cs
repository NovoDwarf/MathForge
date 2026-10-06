using MathForge.Vectors.Integer;

namespace MathForge.Neighborhoods;

public class MooreNeighborhood : INeighborhood2D, INeighborhood3D
{
	private static readonly Int2[] OffsetsInternal2D =
	[
		new(-1, -1), new(0, -1), new(1, -1),
		new(-1, 0),            new(1, 0),
		new(-1, 1), new(0, 1), new(1, 1)
	];

	private static readonly Int3[] OffsetsInternal3D =
	[
		new(-1, -1, -1), new(0, -1, -1), new(1, -1, -1),
		new(-1, 0, -1), new(0, 0, -1), new(1, 0, -1),
		new(-1, 1, -1), new(0, 1, -1), new(1, 1, -1),

		new(-1, -1, 0), new(0, -1, 0), new(1, -1, 0),
		new(-1, 0, 0), new(1, 0, 0),
		new(-1, 1, 0), new(0, 1, 0), new(1, 1, 0),

		new(-1, -1, 1), new(0, -1, 1), new(1, -1, 1),
		new(-1, 0, 1), new(0, 0, 1), new(1, 0, 1),
		new(-1, 1, 1), new(0, 1, 1), new(1, 1, 1)
	];

	public static MooreNeighborhood Default { get; } = new();
	
	private MooreNeighborhood()
	{
	}
	
	public IReadOnlyList<Int2> Offsets2D => OffsetsInternal2D;

	public IReadOnlyList<Int3> Offsets3D => OffsetsInternal3D;
	
	public IEnumerable<Int2> GetNeighbors(int x, int y)
	{
		foreach (var offset in OffsetsInternal2D)
		{
			yield return new Int2(x + offset.X, y + offset.Y);
		}
	}

	public IEnumerable<Int2> GetNeighbors(Int2 coordinate)
	{
		return GetNeighbors(coordinate.X, coordinate.Y);
	}

	public IEnumerable<Int3> GetNeighbors(int x, int y, int z)
	{
		foreach (var offset in OffsetsInternal3D)
		{
			yield return new Int3(x + offset.X, y + offset.Y, z + offset.Z);
		}
	}

	public IEnumerable<Int3> GetNeighbors(Int3 coordinate)
	{
		return GetNeighbors(coordinate.X, coordinate.Y, coordinate.Z);
	}
}