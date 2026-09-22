using MathForge.Core.Entities;

namespace MathForge.Cryptography.Classical.Monoalphabetic;

public sealed class SimpleSubstitutionCipher : Cipher
{
	private readonly byte[] _encryptionMap;
	private readonly byte[] _decryptionMap;

	public SimpleSubstitutionCipher(IReadOnlyDictionary<byte, byte> substitutionMap)
	{
		ArgumentNullException.ThrowIfNull(substitutionMap);

		_encryptionMap = CreateMap(substitutionMap);
		_decryptionMap = CreateInverseMap(_encryptionMap);
	}

	public override byte[] Encrypt(ReadOnlySpan<byte> data)
	{
		var encrypted = new byte[data.Length];

		for (var i = 0; i < data.Length; i++)
			encrypted[i] = _encryptionMap[data[i]];

		return encrypted;
	}

	public override byte[] Decrypt(ReadOnlySpan<byte> data)
	{
		var decrypted = new byte[data.Length];

		for (var i = 0; i < data.Length; i++)
			decrypted[i] = _decryptionMap[data[i]];

		return decrypted;
	}

	private static byte[] CreateMap(IReadOnlyDictionary<byte, byte> substitutionMap)
	{
		var map = new byte[256];

		for (var i = 0; i < map.Length; i++)
			map[i] = (byte)i;

		foreach (var pair in substitutionMap)
			map[pair.Key] = pair.Value;

		return map;
	}

	private static byte[] CreateInverseMap(byte[] map)
	{
		var inverse = new byte[256];
		var used = new bool[256];

		for (var i = 0; i < map.Length; i++)
		{
			var value = map[i];

			if (used[value])
				throw new ArgumentException("The substitution map must be bijective.");

			used[value] = true;
			inverse[value] = (byte)i;
		}

		return inverse;
	}
}