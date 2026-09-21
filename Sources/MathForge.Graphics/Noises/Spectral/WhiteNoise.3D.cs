namespace MathForge.Graphics.Noises.Spectral;

public partial class WhiteNoise
{
	public float Sample(float x, float y, float z)
	{
		x = (x + _options.Offset.X) * _options.Scale;
		y = (y + _options.Offset.Y) * _options.Scale;
		z = (z + _options.Offset.Z) * _options.Scale;

		var hash = Hash(
			x * 374761393f +
			y * 668265263f +
			z * 1442695041f +
			_options.Seed * 2147483647f);

		return ApplyOutput(ToNoise(hash));
	}
}