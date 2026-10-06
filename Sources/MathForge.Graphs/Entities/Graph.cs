using MathForge.Graphs.Interfaces;

namespace MathForge.Graphs.Entities;

public abstract class Graph<TVertex, TEdge> : IMutableGraph<TVertex, TEdge>
	where TVertex : notnull
	where TEdge : IEdge<TVertex>
{
	protected readonly HashSet<TVertex> VertexSet = [];
	protected readonly HashSet<TEdge> EdgeSet = [];

	protected readonly Dictionary<TVertex, HashSet<TVertex>> Adjacency = [];
	protected readonly Dictionary<TVertex, HashSet<TEdge>> OutgoingEdges = [];
	
	public IEnumerable<TVertex> Vertices => VertexSet;
	public IEnumerable<TEdge> Edges => EdgeSet;

	public int VertexCount => VertexSet.Count;

	public int EdgeCount => EdgeSet.Count;
	
	public abstract void AddEdge(TEdge edge);

	public virtual void AddVertex(TVertex vertex)
	{
		if (!VertexSet.Add(vertex))
			return;

		Adjacency.Add(vertex, []);
		OutgoingEdges.Add(vertex, []);
	}
	
	public virtual IEnumerable<TVertex> GetNeighbors(TVertex vertex)
	{
		return Adjacency.TryGetValue(vertex, out var neighbors)
			? neighbors
			: [];
	}

	public virtual IEnumerable<TEdge> GetOutgoingEdges(TVertex vertex)
	{
		return OutgoingEdges.TryGetValue(vertex, out var edges)
			? edges
			: [];
	}

	public virtual bool RemoveVertex(TVertex vertex)
	{
		if (!VertexSet.Remove(vertex))
			return false;

		Adjacency.Remove(vertex);

		foreach (var source in Adjacency.Keys)
			Adjacency[source].Remove(vertex);

		EdgeSet.RemoveWhere(edge => EqualityComparer<TVertex>.Default.Equals(edge.Source, vertex) || EqualityComparer<TVertex>.Default.Equals(edge.Target, vertex));

		return true;
	}

	public virtual bool RemoveEdge(TEdge edge)
	{
		if (!EdgeSet.Remove(edge))
			return false;

		if (Adjacency.TryGetValue(edge.Source, out var neighbors))
			neighbors.Remove(edge.Target);

		return true;
	}

	public bool ContainsVertex(TVertex vertex)
	{
		return VertexSet.Contains(vertex);
	}

	public bool ContainsEdge(TEdge edge)
	{
		return EdgeSet.Contains(edge);
	}
}