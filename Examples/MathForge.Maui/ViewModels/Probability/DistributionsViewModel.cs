using System.Collections.ObjectModel;
using System.Reflection;
using LocalizationResourceManager.Maui;
using MathForge.Core.Attributes;
using MathForge.Core.Base.Entities;
using MathForge.Maui.Core.ViewModels;
using MathForge.Maui.Systems.Algorithms.Services;
using MathForge.Maui.Views.Common;
using Microsoft.Extensions.Logging;

namespace MathForge.Maui.ViewModels.Probability;

public partial class DistributionsViewModel : BaseViewModel
{
	private readonly ILogger<DistributionsViewModel> _logger;
	private readonly ILocalizationResourceManager _manager;
	private readonly DistributionService _distributionService;

	public ObservableCollection<TreeNodeViewModel> DistributionGroups { get; } = [];

	public DistributionsViewModel(
		ILogger<DistributionsViewModel> logger,
		ILocalizationResourceManager manager,
		DistributionService distributionService)
	{
		_logger = logger;
		_manager = manager;
		_distributionService = distributionService;

		LoadAsync();
	}

	public void LoadAsync()
	{
		try
		{
			DistributionGroups.Clear();

			var tree = TreeBuilder.Build(
				items: _distributionService.Distributions,
				pathIdSelector: PathSelector,
				titleSelector: item => item.Name,
				descriptionSelector: item => item.Description,
				options: new TreeBuilderOptions<Distribution>
				{
					DuplicatePolicy = DuplicateLeafPolicy.Ignore,
				}
			);
			
			foreach (var group in tree)
				DistributionGroups.Add(group);
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, "Error while loading distributions");
		}
	}

	private IReadOnlyList<string> PathSelector(Distribution item)
		=> item.GetType().GetCustomAttribute<CategoriesAttribute>()?.Path.Skip(1).ToArray() ?? [];

	private void OnTapped(Distribution distribution) 
		=> _distributionService.Open(distribution);
}