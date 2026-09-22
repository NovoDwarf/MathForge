namespace MathForge.Core.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class EntityParameterAttribute : Attribute
{
	public EntityParameterAttribute(string name)
	{
		NameKey = $"Param_{name}_Name";
		DescKey = $"Param_{name}_Desc";
	}

	public string NameKey { get; }
	public string DescKey { get; }
}