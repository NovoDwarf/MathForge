
using CommunityToolkit.Mvvm.ComponentModel;

namespace MathForge.Maui.Core.ViewModels;

public partial class BaseViewModel : ObservableObject
{
	public virtual IDictionary<string, object?> Save() => new Dictionary<string, object?>();

	public virtual void Load(IDictionary<string, object?> parameters)
	{

	}

	public virtual void Cleanup()
	{
		
	}
}