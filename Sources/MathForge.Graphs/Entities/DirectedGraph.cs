using MathForge.Graphs.Interfaces;

namespace MathForge.Graphs.Entities;

public sealed class DirectedGraph<TVertex> : Graph<TVertex, Edge<TVertex>>, IDirectedGraph<TVertex, Edge<TVertex>>
	where TVertex : notnull
{
	public override void AddEdge(Edge<TVertex> edge)
	{
		AddVertex(edge.Source);
		AddVertex(edge.Target);

		if (!EdgeSet.Add(edge))
			return;

		Adjacency[edge.Source].Add(edge.Target);
		OutgoingEdges[edge.Source].Add(edge);
	}

	public void AddEdge(TVertex source, TVertex target)
	{
		AddEdge(new Edge<TVertex>(source, target));
	}
}