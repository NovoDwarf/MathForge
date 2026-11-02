namespace MathForge.Processing.Search;

public readonly record struct SearchStep<T>(int Left, int Middle, int Right, T Value, T Current);