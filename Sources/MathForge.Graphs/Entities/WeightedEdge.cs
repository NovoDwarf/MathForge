using MathForge.Graphs.Interfaces;

namespace MathForge.Graphs.Entities;

public readonly record struct WeightedEdge<TVertex, TWeight>(TVertex Source, TVertex Target, TWeight Weight) : IEdge<TVertex>;