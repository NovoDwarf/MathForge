using MathForge.Maui.Systems.Application.Constants;
using MathForge.Maui.ViewModels.Pages;

namespace MathForge.Maui.Views;

public partial class SettingsPage : ContentPage
{
	public SettingsPage(SettingsViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}

	public string Route => Routes.SettingsRoute;
}