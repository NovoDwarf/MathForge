using MathForge.Maui.ViewModels.Utilities.Tools;

namespace MathForge.Maui.Views.Modules.Utilities.Tools;

public partial class CodeScannerPage : ContentPage
{
	public CodeScannerPage(CodeScannerViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}