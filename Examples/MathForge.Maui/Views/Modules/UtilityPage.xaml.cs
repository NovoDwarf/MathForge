using MathForge.Maui.Systems.Hosting.Interfaces;
using MathForge.Maui.ViewModels.Utilities;

namespace MathForge.Maui.Views.Modules;

public partial class UtilityPage : ContentPage, ILightPage
{
	public UtilityPage(UtilityViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
	
	public string Route => "Algorithms/Utility";
	

}