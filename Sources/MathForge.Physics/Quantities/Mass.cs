namespace MathForge.Physics;

/// <summary>A scalar mass in kilograms. Dynamic particles require a finite positive mass.</summary>
public readonly record struct Mass(double Kilograms);
