using MathForge.Maui.Systems.Application.Constants;
using MathForge.Maui.Systems.Hosting.Interfaces;
using MathForge.Maui.ViewModels.Pages;

namespace MathForge.Maui.Views;

public partial class MainPage : ContentPage, ILightPage
{
	public MainPage(MainViewModel mainViewModel)
	{
		BindingContext = mainViewModel;
		InitializeComponent();
	}

	public string Route => Routes.MainRoute;
}