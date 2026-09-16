using MathForge.Maui.Core.Views;
using MathForge.Maui.Systems.Application.Constants;
using MathForge.Maui.ViewModels.Utilities.Organizers;

namespace MathForge.Maui.Views.Modules.Utilities.Organizers;

public partial class ConverterPage : MemorablePage
{
	private ConverterViewModel ViewModel => (ConverterViewModel)BindingContext;
	
	public ConverterPage(ConverterViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
	
	public override string Route => Routes.ConverterRoute;
	
	public override void OnLoad(IDictionary<string, object?> parameters) => ViewModel.Load(parameters);

	public override IDictionary<string, object?> OnSave() => ViewModel.Save();
}