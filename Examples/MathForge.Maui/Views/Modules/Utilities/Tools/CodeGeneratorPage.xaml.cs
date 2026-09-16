using MathForge.Maui.ViewModels.Utilities.Tools;

namespace MathForge.Maui.Views.Modules.Utilities.Tools;

public partial class CodeGeneratorPage : ContentView
{
	public CodeGeneratorPage(CodeGeneratorViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}