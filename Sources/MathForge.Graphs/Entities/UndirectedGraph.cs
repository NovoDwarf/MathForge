namespace MathForge.Graphs.Entities;

public sealed class UndirectedGraph<TVertex>
	: Graph<TVertex, Edge<TVertex>>
	where TVertex : notnull
{
	public override void AddEdge(Edge<TVertex> edge)
	{
		AddVertex(edge.Source);
		AddVertex(edge.Target);

		if (!EdgeSet.Add(edge))
			return;

		Adjacency[edge.Source].Add(edge.Target);
		Adjacency[edge.Target].Add(edge.Source);
	}

	public void AddEdge(TVertex first, TVertex second)
	{
		AddEdge(new Edge<TVertex>(first, second));
	}

	public override bool RemoveEdge(Edge<TVertex> edge)
	{
		if (!EdgeSet.Remove(edge))
			return false;

		Adjacency[edge.Source].Remove(edge.Target);
		Adjacency[edge.Target].Remove(edge.Source);

		return true;
	}
}