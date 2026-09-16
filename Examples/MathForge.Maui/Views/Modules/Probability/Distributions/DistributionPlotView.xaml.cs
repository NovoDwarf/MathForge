using MathForge.Core.Base.Entities;
using MathForge.Maui.ViewModels.Probability.Distributions;

namespace MathForge.Maui.Views.Modules.Probability.Distributions;

public partial class DistributionPlotView : ContentView
{
	public static readonly BindableProperty DistributionProperty =
		BindableProperty.Create(nameof(Distribution), typeof(Distribution), typeof(DistributionPlotView));

	public DistributionPlotView()
	{
		BindingContext = new DistributionPlotViewModel();

		InitializeComponent();
	}

	public Distribution Distribution
	{
		get => (Distribution)GetValue(DistributionProperty);
		set => SetValue(DistributionProperty, value);
	}
}