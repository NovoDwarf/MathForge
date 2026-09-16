using MathForge.Maui.ViewModels.Utilities.Sensors;

namespace MathForge.Maui.Views.Modules.Utilities.Sensors;

public partial class BarometerPage : ContentPage
{
	private BarometerViewModel ViewModel => BindingContext as BarometerViewModel ?? throw new InvalidOperationException();

	public BarometerPage(BarometerViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}

	protected override void OnDisappearing()
	{
		//ViewModel.Stop();

		base.OnDisappearing();
	}
}