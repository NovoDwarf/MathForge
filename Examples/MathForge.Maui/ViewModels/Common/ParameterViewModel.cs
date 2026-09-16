using CommunityToolkit.Mvvm.ComponentModel;
using MathForge.Maui.Core.ViewModels;

namespace MathForge.Maui.ViewModels.Common;

public sealed partial class ParameterViewModel : BaseViewModel
{
	[ObservableProperty]
	public partial string Name { get; set; } = string.Empty;

	[ObservableProperty]
	public partial string Description { get; set; } = string.Empty;

	public Type? DataType { get; set; }

	[ObservableProperty]
	public partial object? Value { get; set; }

	public double? Min { get; set; }

	public double? Max { get; set; }
}