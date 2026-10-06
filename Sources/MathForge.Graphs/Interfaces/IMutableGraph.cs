namespace MathForge.Graphs.Interfaces;

public interface IMutableGraph<TVertex, TEdge> : IGraph<TVertex, TEdge>
{
	void AddVertex(TVertex vertex);

	void AddEdge(TEdge edge);

	bool RemoveVertex(TVertex vertex);

	bool RemoveEdge(TEdge edge);
}