using System.Numerics;
using MathForge.Vectors.Float;

namespace MathForge.Ballistics.Entities.Data;

public class WorldData
{
	public Float3 Gravity { get; set; } = new(0, -9.81f, 0);
}