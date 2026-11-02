namespace MathForge.Graphs.Interfaces;

public interface IDirectedGraph<TVertex, TEdge> : IGraph<TVertex, TEdge> where TEdge : IEdge<TVertex>
{
}