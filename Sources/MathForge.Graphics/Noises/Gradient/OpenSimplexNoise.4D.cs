using System.Runtime.CompilerServices;
using MathForge.Graphics.Noises.Options;

namespace MathForge.Graphics.Noises.Gradient;

public partial class OpenSimplexNoise
{
	private const long SeedFlip3D = -0x52D547B2E96ED629L;
	private const long SeedOffset4D = 0xE83DC3E0DA7164DL;

	private const float Skew4D = -0.138196601125011f;
	private const float Unskew4D = 0.309016994374947f;
	private const float LatticeStep4D = 0.2f;

	private const int NGrads4DExponent = 9;
	private const int NGrads4D = 1 << NGrads4DExponent;

	private const double Normalizer4D = 0.0220065933241897;
	private const float Rsquared4D = 0.6f;
	private static readonly float[] Gradients4D;

	public float Sample(float x, float y, float z, float w)
	{
		ApplyInput(ref x, ref y, ref z, ref w);

		var value = _options.Type4D switch
		{
			OpenSimplexNoise4DType.Unskewed => Noise4_UnskewedBase(_options.Seed, x, y, z, w),
			OpenSimplexNoise4DType.ImproveXYZ => Noise4_ImproveXYZ(_options.Seed, x, y, z, w),
			OpenSimplexNoise4DType.ImproveXYZ_ImproveXZ => Noise4_ImproveXYZ_ImproveXZ(_options.Seed, x, y, z, w),
			OpenSimplexNoise4DType.ImproveXYZ_ImproveXY => Noise4_ImproveXYZ_ImproveXY(_options.Seed, x, y, z, w),
			OpenSimplexNoise4DType.Fallback => Noise4_Fallback(_options.Seed, x, y, z, w),
			_ => throw new ArgumentOutOfRangeException()
		};

		return ApplyOutput(value);
	}

	private static float Noise4_ImproveXYZ_ImproveXY(long seed, double x, double y, double z, double w)
	{
		var xy = x + y;
		var s2 = xy * -0.21132486540518699998;
		var zz = z * 0.28867513459481294226;
		var ww = w * 0.2236067977499788;

		var xr = x + (zz + ww + s2);
		var yr = y + (zz + ww + s2);
		var zr = xy * -0.57735026918962599998 + (zz + ww);
		var wr = z * -0.866025403784439 + ww;

		return Noise4_UnskewedBase(seed, xr, yr, zr, wr);
	}

	private static float Noise4_ImproveXYZ_ImproveXZ(long seed, double x, double y, double z, double w)
	{
		var xz = x + z;
		var s2 = xz * -0.21132486540518699998;
		var yy = y * 0.28867513459481294226;
		var ww = w * 0.2236067977499788;

		var xr = x + (yy + ww + s2);
		var zr = z + (yy + ww + s2);
		var yr = xz * -0.57735026918962599998 + (yy + ww);
		var wr = y * -0.866025403784439 + ww;

		return Noise4_UnskewedBase(seed, xr, yr, zr, wr);
	}

	private static float Noise4_ImproveXYZ(long seed, double x, double y, double z, double w)
	{
		var xyz = x + y + z;
		var ww = w * 0.2236067977499788;
		var s2 = xyz * -0.16666666666666666 + ww;

		var xs = x + s2;
		var ys = y + s2;
		var zs = z + s2;
		var ws = -0.5 * xyz + ww;

		return Noise4_UnskewedBase(seed, xs, ys, zs, ws);
	}

	private static float Noise4_Fallback(long seed, double x, double y, double z, double w)
	{
		var s = Skew4D * (x + y + z + w);

		var xs = x + s;
		var ys = y + s;
		var zs = z + s;
		var ws = w + s;

		return Noise4_UnskewedBase(seed, xs, ys, zs, ws);
	}

	private static float Noise4_UnskewedBase(long seed, double xs, double ys, double zs, double ws)
	{
		var xsb = FastUtils.Floor(xs);
		var ysb = FastUtils.Floor(ys);
		var zsb = FastUtils.Floor(zs);
		var wsb = FastUtils.Floor(ws);

		var xsi = (float)(xs - xsb);
		var ysi = (float)(ys - ysb);
		var zsi = (float)(zs - zsb);
		var wsi = (float)(ws - wsb);

		var siSum = xsi + ysi + zsi + wsi;
		var startingLattice = (int)(siSum * 1.25);

		seed += startingLattice * SeedOffset4D;

		var startingLatticeOffset = startingLattice * -LatticeStep4D;

		xsi += startingLatticeOffset;
		ysi += startingLatticeOffset;
		zsi += startingLatticeOffset;
		wsi += startingLatticeOffset;

		var ssi = (siSum + startingLatticeOffset * 4) * Unskew4D;

		var xsvp = xsb * PrimeX;
		var ysvp = ysb * PrimeY;
		var zsvp = zsb * PrimeZ;
		var wsvp = wsb * PrimeW;

		var value = 0f;

		for (var i = 0;; i++)
		{
			var score0 = 1.0 + ssi * (-1.0 / Unskew4D);

			if (xsi >= ysi && xsi >= zsi && xsi >= wsi && xsi >= score0)
			{
				xsvp += PrimeX;
				xsi -= 1;
				ssi -= Unskew4D;
			}
			else if (ysi > xsi && ysi >= zsi && ysi >= wsi && ysi >= score0)
			{
				ysvp += PrimeY;
				ysi -= 1;
				ssi -= Unskew4D;
			}
			else if (zsi > xsi && zsi > ysi && zsi >= wsi && zsi >= score0)
			{
				zsvp += PrimeZ;
				zsi -= 1;
				ssi -= Unskew4D;
			}
			else if (wsi > xsi && wsi > ysi && wsi > zsi && wsi >= score0)
			{
				wsvp += PrimeW;
				wsi -= 1;
				ssi -= Unskew4D;
			}

			var dx = xsi + ssi;
			var dy = ysi + ssi;
			var dz = zsi + ssi;
			var dw = wsi + ssi;

			var a = dx * dx + dy * dy + dz * dz + dw * dw;

			if (a < Rsquared4D)
			{
				a -= Rsquared4D;
				a *= a;
				value += a * a * Grad(seed, xsvp, ysvp, zsvp, wsvp, dx, dy, dz, dw);
			}

			if (i == 4)
				break;

			xsi += LatticeStep4D;
			ysi += LatticeStep4D;
			zsi += LatticeStep4D;
			wsi += LatticeStep4D;
			ssi += LatticeStep4D * 4 * Unskew4D;
			seed -= SeedOffset4D;

			if (i != startingLattice)
				continue;

			xsvp -= PrimeX;
			ysvp -= PrimeY;
			zsvp -= PrimeZ;
			wsvp -= PrimeW;
			seed += SeedOffset4D * 5;
		}

		return value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static float Grad(long seed, long xsvp, long ysvp, long zsvp, long wsvp, float dx, float dy, float dz,
		float dw)
	{
		var hash = seed ^ xsvp ^ ysvp ^ zsvp ^ wsvp;
		hash *= HashMultiplier;
		hash ^= hash >> (64 - NGrads4DExponent + 2);

		var gi = (int)hash & ((NGrads4D - 1) << 2);

		return Gradients4D[gi] * dx + Gradients4D[gi | 1] * dy + (Gradients4D[gi | 2] * dz + Gradients4D[gi | 3] * dw);
	}
}