using MathForge.Maui.Systems.Application.Constants;
using MathForge.Maui.Systems.Hosting.Interfaces;

namespace MathForge.Maui.Views;

public partial class ModulesPage : ContentPage, ILightPage
{
	public ModulesPage()
	{
		InitializeComponent();
	}
	
	public string Route => Routes.ModulesRoute;
}