using MathForge.Maui.ViewModels.Utilities.Sensors;

namespace MathForge.Maui.Views.Modules.Utilities.Sensors;

public partial class SpeedometerPage : ContentPage
{
	public SpeedometerPage(SpeedometerViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}