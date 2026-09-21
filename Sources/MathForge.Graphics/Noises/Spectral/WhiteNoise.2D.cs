namespace MathForge.Graphics.Noises.Spectral;

public partial class WhiteNoise
{
	public float Sample(float x, float y)
	{
		x = (x + _options.Offset.X) * _options.Scale;
		y = (y + _options.Offset.Y) * _options.Scale;

		var hash = Hash(x * 374761393f + y * 668265263f + _options.Seed * 1442695040888963407f);

		return ApplyOutput(ToNoise(hash));
	}
}