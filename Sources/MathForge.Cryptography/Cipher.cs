using MathForge.Core;

namespace MathForge.Cryptography;

public abstract class Cipher : MathEntity
{
	public abstract byte[] Encrypt(ReadOnlySpan<byte> data);

	public abstract byte[] Decrypt(ReadOnlySpan<byte> data);
}