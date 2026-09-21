using MathForge.Fields;
using MathForge.Imaging.Colors;
using MathForge.Scalars;

namespace MathForge.Imaging.Bitmaps;

public sealed class RgbaBitmap : IDisposable
{
	public RgbaBitmap(Field<NFloat> r, Field<NFloat> g, Field<NFloat> b, Field<NFloat> a)
	{
		ArgumentNullException.ThrowIfNull(r);
		ArgumentNullException.ThrowIfNull(g);
		ArgumentNullException.ThrowIfNull(b);
		ArgumentNullException.ThrowIfNull(a);

		if (r.Width != g.Width || r.Width != b.Width || r.Width != a.Width ||
		    r.Height != g.Height || r.Height != b.Height || r.Height != a.Height)
		{
			throw new ArgumentException("All channels must have the same dimensions.");
		}

		R = r;
		G = g;
		B = b;
		A = a;
	}

	public Field<NFloat> R { get; }
	public Field<NFloat> G { get; }
	public Field<NFloat> B { get; }
	public Field<NFloat> A { get; }

	public int Width => R.Width;
	public int Height => R.Height;

	public void SetPixel(int x, int y, ColorF color)
	{
		R[x, y] = color.R;
		G[x, y] = color.G;
		B[x, y] = color.B;
		A[x, y] = color.A;
	}
	
	public ColorF GetPixel(int x, int y)
	{
		return new ColorF(
			R[x, y],
			G[x, y],
			B[x, y],
			A[x, y]);
	}
	
	public void Dispose()
	{
		R.Dispose();
		G.Dispose();
		B.Dispose();
		A.Dispose();
	}
}