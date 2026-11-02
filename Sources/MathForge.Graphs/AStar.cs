using MathForge.Graphs.Entities;
using MathForge.Graphs.Interfaces;
using MathForge.Graphs.Results;

namespace MathForge.Graphs;

public sealed class AStar<TVertex> where TVertex : notnull
{
	private readonly IGraph<TVertex, WeightedEdge<TVertex, double>> _graph;
	private readonly Func<TVertex, TVertex, double> _heuristic;

	public AStar(IGraph<TVertex, WeightedEdge<TVertex, double>> graph, Func<TVertex, TVertex, double> heuristic)
	{
		_graph = graph;
		_heuristic = heuristic;
	}

	public ShortestPathResult<TVertex, double> FindPath(TVertex start, TVertex goal)
	{
		var openSet = new PriorityQueue<TVertex, double>();
		var cameFrom = new Dictionary<TVertex, TVertex>();

		var gScores = new Dictionary<TVertex, double> { [start] = 0 };
		var fScores = new Dictionary<TVertex, double> { [start] = _heuristic(start, goal) };
        
		openSet.Enqueue(start, fScores[start]);

		while (openSet.Count > 0)
		{
			var current = openSet.Dequeue();

			if (EqualityComparer<TVertex>.Default.Equals(current, goal))
				return BuildResult(current, start, cameFrom, gScores[current]);

			foreach (var (_, neighbor, weight) in _graph.GetOutgoingEdges(current))
			{
				var tentativeGScore = gScores[current] + weight;

				if (gScores.TryGetValue(neighbor, out var neighborScore) && !(tentativeGScore < neighborScore))
					continue;
	            
				cameFrom[neighbor] = current;
				gScores[neighbor] = tentativeGScore;

				var fScore = tentativeGScore + _heuristic(neighbor, goal);

				openSet.Enqueue(neighbor, fScore);
			}
		}

		return new ShortestPathResult<TVertex, double>(false, double.PositiveInfinity, []);
	}
    
	private static ShortestPathResult<TVertex, double> BuildResult(TVertex current, TVertex start, Dictionary<TVertex, TVertex> cameFrom, double distance)
	{
		var path = new List<TVertex> { current };

		while (!EqualityComparer<TVertex>.Default.Equals(current, start))
		{
			current = cameFrom[current];
			path.Add(current);
		}

		path.Reverse();

		return new ShortestPathResult<TVertex, double>(true, distance, path);
	}
}