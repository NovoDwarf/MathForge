using MathForge.Maui.ViewModels.Utilities.Sensors;

namespace MathForge.Maui.Views.Modules.Utilities.Sensors;

public partial class DeviceInfoPage : ContentPage
{
	public DeviceInfoPage(DeviceInfoViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}