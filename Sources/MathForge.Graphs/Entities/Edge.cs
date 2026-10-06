using MathForge.Graphs.Interfaces;

namespace MathForge.Graphs.Entities;

public readonly record struct Edge<TVertex>(TVertex Source, TVertex Target) : IEdge<TVertex>;