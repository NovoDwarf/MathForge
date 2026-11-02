namespace MathForge.Graphs.Interfaces;

public interface IGraph<TVertex, TEdge>
{
	public IEnumerable<TVertex> Vertices { get; }

	public IEnumerable<TEdge> Edges { get; }

	public IEnumerable<TVertex> GetNeighbors(TVertex vertex);
	
	public IEnumerable<TEdge> GetOutgoingEdges(TVertex vertex);
}