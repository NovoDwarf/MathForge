using MathForge.Maui.ViewModels.Utilities.Organizers;

namespace MathForge.Maui.Views.Modules.Utilities.Organizers;

public partial class StopwatchPage : ContentPage
{
	public StopwatchPage(StopwatchViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}