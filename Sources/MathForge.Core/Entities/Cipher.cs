namespace MathForge.Core.Entities;

public abstract class Cipher : Entity
{
	public abstract byte[] Encrypt(ReadOnlySpan<byte> data);

	public abstract byte[] Decrypt(ReadOnlySpan<byte> data);
}