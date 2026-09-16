using MathForge.Maui.ViewModels.Utilities.Tools;

namespace MathForge.Maui.Views.Modules.Utilities.Tools;

public partial class CashCalculatorPage : ContentPage
{
	public CashCalculatorPage(CashCalculatorViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}