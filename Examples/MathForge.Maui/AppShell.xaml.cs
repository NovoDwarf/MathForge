using MathForge.Maui.Systems.Application.Handlers;

namespace MathForge.Maui;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		
		RoutingHandler.Register(typeof(AppShell).Assembly);
	}
}