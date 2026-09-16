using MathForge.Core.Base.Entities;
using MathForge.Maui.Views.Modules.Probability.Distributions;
using MathForge.Probability.Distributions.Univariate.Discrete.Finite;

namespace MathForge.Maui.Systems.Algorithms.Services;

public sealed class DistributionService 
{
	private readonly Dictionary<Type, object> _viewModels = [];
	
	public DistributionService()
	{
		Distributions = typeof(BernoulliDistribution).Assembly
			.GetTypes()
			.Where(t => !t.IsAbstract && typeof(Distribution).IsAssignableFrom(t))
			.Select(t => (Distribution)Activator.CreateInstance(t)!)
			.ToList();
		
		Initialize(Distributions);
	}

	public IEnumerable<Distribution> Distributions { get; }
	
	public void Open(Distribution distribution)
	{
		var distType = distribution.GetType();

		if (!_viewModels.TryGetValue(distType, out var viewModel))
			throw new InvalidOperationException($"DistributionViewModel for {distType.Name} not initialized");

		var page = new DistributionPage
		{
			BindingContext = viewModel
		};

		Shell.Current.Navigation.PushAsync(page);
	}
	
	private void Initialize(IEnumerable<Distribution> distributions)
	{
		_viewModels.Clear();

		foreach (var distribution in distributions)
		{
			var distType = distribution.GetType();

			if (_viewModels.ContainsKey(distType))
				continue;

			var vmType = typeof(global::MathForge.Maui.ViewModels.Probability.Distributions.DistributionViewModel<>)
				.MakeGenericType(distType);

			var viewModel = Activator.CreateInstance(vmType, distribution);

			_viewModels[distType] = viewModel!;
		}
	}
}