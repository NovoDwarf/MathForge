using System.ComponentModel.DataAnnotations;

namespace MathForge.Graphics.Noises.Options;

public record PerlinNoiseOptions : SeedNoiseOptions
{
	[Range(1, 4096, ErrorMessage = "Size must be a power of 2.")]
	public int Size { get; init; } = 256;
}