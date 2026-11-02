using MathForge.Graphs.Interfaces;
using MathForge.Graphs.Results;

namespace MathForge.Graphs;

public static class TopologicalSort
{
	public static TopologicalSortResult<TVertex> Sort<TVertex, TEdge>(IGraph<TVertex, TEdge> graph) where TEdge : IEdge<TVertex> where TVertex : notnull
	{
		var inDegree = new Dictionary<TVertex, int>();

		foreach (var vertex in graph.Vertices)
			inDegree[vertex] = 0;

		foreach (var vertex in graph.Vertices)
		foreach (var neighbor in graph.GetNeighbors(vertex))
			inDegree[neighbor]++;

		var queue = new Queue<TVertex>(inDegree.Where(static x => x.Value == 0).Select(static x => x.Key));
		var order = new List<TVertex>(inDegree.Count);

		while (queue.Count > 0)
		{
			var vertex = queue.Dequeue();

			order.Add(vertex);

			foreach (var neighbor in graph.GetNeighbors(vertex))
			{
				inDegree[neighbor]--;

				if (inDegree[neighbor] == 0)
					queue.Enqueue(neighbor);
			}
		}

		return new TopologicalSortResult<TVertex>(order.Count == inDegree.Count, order);
	}
}