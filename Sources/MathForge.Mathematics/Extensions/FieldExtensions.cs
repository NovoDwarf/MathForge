using MathForge.Fields;

namespace MathForge.Extensions;

public static class FieldExtensions
{
	public static Field<T> Fill<T>(int width, int height, T value)
	{
		var field = new Field<T>(width, height);

		field.Values.Fill(value);

		return field;
	}
}