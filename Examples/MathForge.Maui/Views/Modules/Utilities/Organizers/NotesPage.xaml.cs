using MathForge.Maui.ViewModels.Utilities.Organizers;

namespace MathForge.Maui.Views.Modules.Utilities.Organizers;

public partial class NotesPage : ContentPage
{
	public NotesPage(NotesViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}