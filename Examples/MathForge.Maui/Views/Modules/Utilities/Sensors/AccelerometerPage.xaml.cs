using MathForge.Maui.ViewModels.Utilities.Sensors;

namespace MathForge.Maui.Views.Modules.Utilities.Sensors;

public partial class AccelerometerPage : ContentPage
{
	private AccelerometerViewModel ViewModel => BindingContext as AccelerometerViewModel ?? throw new InvalidOperationException();

	public AccelerometerPage(AccelerometerViewModel viewModel)
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