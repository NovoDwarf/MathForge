namespace MathForge.Core;

/// <summary>
/// Represents a mathematical entity.
/// </summary>
public abstract class MathEntity
{
	public Guid Id { get; } = Guid.NewGuid();
	
	public virtual string Name => "Name";
	
	public virtual string Description => "Desc";

	public virtual void Set(params object[] parameters) { }
}
