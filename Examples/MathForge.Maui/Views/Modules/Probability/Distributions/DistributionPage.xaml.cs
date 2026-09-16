using MathForge.Core.Base.Entities;
using MathForge.Maui.ViewModels.Probability.Distributions;

namespace MathForge.Maui.Views.Modules.Probability.Distributions;

public interface IHasDistribution
{
	public Distribution Distribution { get; }

	public event Action<Distribution>? DistributionChanged;
}

public partial class DistributionPage : ContentPage
{
	private IHasDistribution ViewModel => (IHasDistribution)BindingContext;
	private DistributionPlotViewModel? _plotViewModel;

	public DistributionPage()
	{
		InitializeComponent();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();

		if (PlotView.BindingContext is not DistributionPlotViewModel plotVm)
			return;

		_plotViewModel = plotVm;
		_plotViewModel.Subscribe(ViewModel);
	}

	protected override void OnDisappearing()
	{
		base.OnDisappearing();

		_plotViewModel?.Unsubscribe();
	}
}