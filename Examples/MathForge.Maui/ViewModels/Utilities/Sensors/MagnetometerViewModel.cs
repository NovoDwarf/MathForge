using MathForge.Maui.Core.ViewModels;
using MathForge.Maui.Systems.Application.Constants;
using MathForge.Maui.Systems.Sensors.Services;

namespace MathForge.Maui.ViewModels.Utilities.Sensors;

public class MagnetometerViewModel : BaseViewModel
{
	public MagnetometerViewModel(MagnetometerService magnetometerService)
	{
		
	}

	private void Start()
	{
		if (!MagnetometerService.IsSupported)
		{ 
			Alerts.SensorNotSupported("Accelerometer");
			return;
		}
	}
	
	private void Stop()
	{
		
	}
}