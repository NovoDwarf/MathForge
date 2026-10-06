namespace MathForge.Utilities;

public static class FloatUtils
{
    public const float AbsTol = 1e-6f;
    public const float RelTol = 1e-6f;

    public static bool Approximately(float a, float b)
        => NumericUtils<float>.ApproximatelySmart(a, b, AbsTol, RelTol);
}