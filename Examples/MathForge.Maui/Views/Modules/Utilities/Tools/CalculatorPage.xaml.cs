using MathForge.Maui.ViewModels.Utilities.Tools;

namespace MathForge.Maui.Views.Modules.Utilities.Tools;

public partial class CalculatorPage : ContentPage
{
	public CalculatorPage(CalculatorViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}