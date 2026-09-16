using MathForge.Core.Base.Graphics;

namespace MathForge.Core.Interfaces.Drawings;

public interface ILineClipper
{
	public Rect Rect { get; set; }

	public bool Clip(ref float x0, ref float y0, ref float x1, ref float y1);
}