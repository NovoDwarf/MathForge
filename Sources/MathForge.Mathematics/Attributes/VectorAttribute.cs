namespace MathForge.Attributes;

[AttributeUsage(AttributeTargets.Struct)]
internal sealed class VectorAttribute(int dimension) : Attribute
{
	public int Dimension { get; } = dimension;
}