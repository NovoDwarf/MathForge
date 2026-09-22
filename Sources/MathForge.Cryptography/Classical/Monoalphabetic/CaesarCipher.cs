using MathForge.Core.Entities;

namespace MathForge.Cryptography.Classical.Monoalphabetic;

public sealed class CaesarCipher : Cipher
{
	private readonly int _shift;

	public CaesarCipher(int shift)
	{
		_shift = shift;
	}

	public override byte[] Encrypt(ReadOnlySpan<byte> bytes)
	{
		var result = new byte[bytes.Length];

		for (var i = 0; i < bytes.Length; i++)
			result[i] = Transform(bytes[i], _shift);

		return result;
	}

	public override byte[] Decrypt(ReadOnlySpan<byte> bytes)
	{
		var result = new byte[bytes.Length];

		for (var i = 0; i < bytes.Length; i++)
			result[i] = Transform(bytes[i], -_shift);

		return result;
	}

	private static byte Transform(byte value, int shift)
	{
		return (byte)((value + shift % 256 + 256) % 256);
	}
}