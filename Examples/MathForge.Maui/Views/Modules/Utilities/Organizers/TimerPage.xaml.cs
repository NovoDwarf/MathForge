using MathForge.Maui.ViewModels.Utilities.Organizers;

namespace MathForge.Maui.Views.Modules.Utilities.Organizers;

public partial class TimerPage : ContentPage
{
	public TimerPage(TimerViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}