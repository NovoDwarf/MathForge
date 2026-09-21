namespace MathForge.Random.Generators;

public interface IRandom
{
	ulong NextUInt64();

	uint NextUInt32();

	int Next();

	int Next(int max);

	int Next(int min, int max);

	void NextBytes(Span<byte> buffer);
	
	double NextDouble();

	double NextDouble(double min, double max);
	
	float NextSingle();

	bool NextBoolean();
}