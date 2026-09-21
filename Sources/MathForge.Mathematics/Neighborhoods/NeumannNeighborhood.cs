using MathForge.Vectors.Integer;

namespace MathForge.Neighborhoods;

public class NeumannNeighborhood : INeighborhood2D, INeighborhood3D
{
	private static readonly Int2[] OffsetsInternal2D =
	[
		new(0, -1),
		new(-1, 0), new(1, 0),
		new(0, 1)
	];

	private static readonly Int3[] OffsetsInternal3D =
	[
		new(0, 0, -1),
		new(0, 0, 1),
		new(0, -1, 0),
		new(0, 1, 0),
		new(-1, 0, 0),
		new(1, 0, 0)
	];

	private NeumannNeighborhood()
	{
	}

	public static NeumannNeighborhood Default { get; } = new();

	public IReadOnlyList<Int2> Offsets2D => OffsetsInternal2D;

	public IReadOnlyList<Int3> Offsets3D => OffsetsInternal3D;
}