using CommunityToolkit.Mvvm.ComponentModel;

namespace MathForge.Maui.Systems.Algorithms.Entities;

public sealed class LapItem : ObservableObject
{
	public string Index { get; set; } = string.Empty;
	public string Time { get; set; } = string.Empty;
	public string Delta { get; set; } = string.Empty;
}