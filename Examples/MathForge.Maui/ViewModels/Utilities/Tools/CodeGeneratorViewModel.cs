using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MathForge.Maui.Core.ViewModels;

namespace MathForge.Maui.ViewModels.Utilities.Tools;

public partial class CodeGeneratorViewModel : BaseViewModel
{
    [ObservableProperty]
    public partial string InputText { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string SelectedCodeType { get; set; }

    [ObservableProperty]
    public partial ImageSource GeneratedImage { get; set; }

    public string[] CodeTypes { get; } =
    [
	    "QR Code", "DataMatrix", "Aztec", "PDF417", "Code128"
    ];

	public CodeGeneratorViewModel()
	{
		SelectedCodeType = CodeTypes[0];
	}

	[RelayCommand]
	private async Task Generate()
	{
		if (string.IsNullOrWhiteSpace(InputText))
		{
			await Shell.Current.DisplayAlertAsync("Ошибка", "Введите данные для генерации кода", "OK");
			return;
		}
	}
}