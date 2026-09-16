using MathForge.Maui.Systems.Hosting.Interfaces;

namespace MathForge.Maui.Views.Modules;

public partial class CompressionsPage : ContentPage, ILightPage
{
	public CompressionsPage()
	{
		InitializeComponent();
	}
	
	public string Route => "Algorithms/Compressions";
}