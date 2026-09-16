using CommunityToolkit.Mvvm.ComponentModel;

namespace MathForge.Maui.Systems.Algorithms.Entities;

public class NoteItem : ObservableObject
{
	public string Title { get; set; } = string.Empty;
	public string Content { get; set; } = string.Empty;
}