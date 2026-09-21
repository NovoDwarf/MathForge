using MathForge.Scalars;

namespace MathForge.Imaging.Colors;

public readonly record struct ColorF(NFloat R, NFloat G, NFloat B, NFloat A)
{
	public ColorF(NFloat r, NFloat g, NFloat b) : this(r, g, b, NFloat.One) { }
	
	public static ColorF Transparent => new(0f, 0f, 0f, 0f);
	public static ColorF Black => new(0f, 0f, 0f, 1f);
	public static ColorF White => new(1f, 1f, 1f, 1f);
	public static ColorF Red => new(1f, 0f, 0f, 1f);
	public static ColorF Green => new(0f, 1f, 0f, 1f);
	public static ColorF Blue => new(0f, 0f, 1f, 1f);
	public static ColorF Yellow => new(1f, 1f, 0f, 1f);
	public static ColorF Cyan => new(0f, 1f, 1f, 1f);
	public static ColorF Magenta => new(1f, 0f, 1f, 1f);
	
	
}