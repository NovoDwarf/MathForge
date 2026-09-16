using MathForge.Maui.Systems.Hosting.Interfaces;

namespace MathForge.Maui.Views.Modules;

public partial class NumericalPage : ContentPage, ILightPage
{
	public NumericalPage()
	{
		InitializeComponent();
	}
	
	public string Route => "Algorithms/Functions";
	
}