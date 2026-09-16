using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using MathForge.Maui.Core.ViewModels;
using MathForge.Maui.Systems.Application.Constants;
using MathForge.Maui.Systems.Sensors.Processing;
using MathForge.Maui.Systems.Sensors.Services;
using MathForge.Maui.Systems.Utilities;
using SkiaSharp;

namespace MathForge.Maui.ViewModels.Utilities.Sensors;

public sealed partial class BarometerViewModel : BaseViewModel
{
    private readonly BarometerService _sensor;
    private readonly BarometerProcessingService _processing;

    public BarometerViewModel(BarometerService sensor, BarometerProcessingService processing)
    {
        _sensor = sensor;
        _processing = processing;

        PressureSeries =
        [
            new LineSeries<double>
            {
                Values = [],
                GeometrySize = 0,
                Stroke = new SolidColorPaint(SKColors.DeepSkyBlue, 2),
                Fill = null
            }
        ];

        _sensor.Reading += OnReading;
    }

    public ISeries[] PressureSeries { get; }

    [ObservableProperty] public partial double Pressure { get; set; }
    [ObservableProperty] public partial double Altitude { get; set; }
    [ObservableProperty] public partial string Trend { get; set; } = "—";
    [ObservableProperty] public partial bool IsActive { get; set; }

    [RelayCommand]
    private void Toggle()
    {
        if (IsActive)
            Stop();
        else
            Start();
    }

    private void Start()
    {
        if (!BarometerService.IsSupported)
        {
            Alerts.SensorNotSupported("");
            return;
        }

        _sensor.Start();
        IsActive = true;
    }

    private void Stop()
    {
        _sensor.Stop();
        IsActive = false;
    }

    private void OnReading(object? sender, double raw)
    {
        var data = _processing.Process(raw);

        MainThread.BeginInvokeOnMainThread(() =>
        {
            Pressure = Math.Round(data.Pressure, 2);
            Altitude = Math.Round(data.Altitude, 1);
            Trend = data.Trend;

            ChartUtils.Push(PressureSeries[0], data.Pressure);
        });
    }
}

