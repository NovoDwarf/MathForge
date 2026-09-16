using MathForge.Maui.Systems.Hosting.Interfaces;
using MathForge.Maui.ViewModels.Probability;

namespace MathForge.Maui.Views.Modules;

public partial class ProbabilityPage : ContentPage, ILightPage
{
	public ProbabilityPage(DistributionsViewModel viewModel)
	{
		BindingContext = viewModel;

		InitializeComponent();
	}
	
	public string Route => "Algorithms/Distributions";
	

}