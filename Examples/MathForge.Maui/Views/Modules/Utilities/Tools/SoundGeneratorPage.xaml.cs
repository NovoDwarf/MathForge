using MathForge.Maui.ViewModels.Utilities.Tools;

namespace MathForge.Maui.Views.Modules.Utilities.Tools;

public partial class SoundGeneratorPage : ContentPage
{
	public SoundGeneratorPage(SoundGeneratorViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}