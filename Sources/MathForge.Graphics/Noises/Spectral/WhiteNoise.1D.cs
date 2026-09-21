namespace MathForge.Graphics.Noises.Spectral;

public partial class WhiteNoise
{
	public float Sample(float x)
	{
		x = (x + _options.Offset.X) * _options.Scale;

		var hash = Hash(x * 374761393f + _options.Seed * 668265263f);

		return ApplyOutput(ToNoise(hash));
	}
}