using MathForge.Maui.Core.ViewModels;
using MathForge.Maui.Views.Graphics;

namespace MathForge.Maui.ViewModels.Utilities.Tools;

public class SynthViewModel : BaseViewModel
{
	public PianoRollDrawable PianoRollDrawable { get; } = new();
}