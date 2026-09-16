using MathForge.Maui.ViewModels.Utilities.Sensors;

namespace MathForge.Maui.Views.Modules.Utilities.Sensors;

public partial class CompassPage : ContentPage
{
	private CompassViewModel ViewModel => BindingContext as CompassViewModel ?? throw new NullReferenceException();

	public CompassPage(CompassViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}

	protected override void OnDisappearing()
	{
		base.OnDisappearing();
	}
}