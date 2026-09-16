using MathForge.Maui.Systems.Application.Services;
using MathForge.Maui.Systems.Hosting.Services;

namespace MathForge.Maui.Core.Views;

public abstract class MemorablePage : NavigationPage, IStateable
{
	protected ActivityService ActivityService { get; }

	protected MemorablePage()
	{
		ActivityService = InjectionService.Resolve<ActivityService>();
	}
	
	public abstract void OnLoad(IDictionary<string, object?> parameters);

	public abstract IDictionary<string, object?> OnSave();

	protected override void OnDisappearing()
	{
		base.OnDisappearing();
		
		ArgumentException.ThrowIfNullOrWhiteSpace(Route);
		
		ActivityService.SaveState(Route, OnSave());
		ActivityService.MarkRecent(Route);
	}
}