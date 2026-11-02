using System.Numerics;

namespace MathForge.LinearAlgebra.Matrices;

public sealed class Matrix<T>
	where T : INumber<T>
{
	private readonly T[] _elements;

	public int Rows { get; }
	public int Columns { get; }

	public Matrix(int rows, int columns)
	{
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(rows);
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(columns);

		Rows = rows;
		Columns = columns;

		_elements = new T[checked(rows * columns)];
	
		Array.Fill(_elements, T.Zero);
	}

	public T this[int row, int column]
	{
		get => _elements[GetIndex(row, column)];
		set => _elements[GetIndex(row, column)] = value;
	}

	private int GetIndex(int row, int column)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(row);
		ArgumentOutOfRangeException.ThrowIfNegative(column);

		ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(row, Rows);

		ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(column, Columns);

		return row * Columns + column;
	}
}