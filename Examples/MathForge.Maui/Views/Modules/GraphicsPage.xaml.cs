using MathForge.Maui.Systems.Hosting.Interfaces;

namespace MathForge.Maui.Views.Modules;

public partial class GraphicsPage : ContentPage, ILightPage
{
	public GraphicsPage()
	{
		InitializeComponent();
	}
	
	public string Route => "Algorithms/Graphics";
	

}