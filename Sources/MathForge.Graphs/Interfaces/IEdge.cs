namespace MathForge.Graphs.Interfaces;

public interface IEdge<TVertex>
{
	TVertex Source { get; }

	TVertex Target { get; }
}