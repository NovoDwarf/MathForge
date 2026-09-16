using MathForge.Maui.Systems.Application.Services;

namespace MathForge.Maui.Core.Views;

public abstract class NavigationPage : ContentPage, INavigable
{
	public abstract string Route { get; }
}