using System.Diagnostics;
using System.Reflection;
using LocalizationResourceManager.Maui;
using MathForge.Core.Resources;
using MathForge.Maui.Resources.Localizations;
using MathForge.Maui.Systems.Algorithms.Services;
using MathForge.Maui.Systems.Application.Handlers;
using MathForge.Maui.Systems.Application.Services;
using MathForge.Maui.Systems.Hosting.Interfaces;
using MathForge.Maui.Systems.Measurements.Interfaces;
using MathForge.Maui.Systems.Measurements.Services;
using MathForge.Maui.Systems.Sensors.Processing;
using MathForge.Maui.Systems.Sensors.Services;
using MathForge.Maui.ViewModels.Pages;
using MathForge.Maui.ViewModels.Probability;
using MathForge.Maui.ViewModels.Utilities;
using MathForge.Maui.ViewModels.Utilities.Organizers;
using MathForge.Maui.ViewModels.Utilities.Sensors;
using MathForge.Maui.ViewModels.Utilities.Tools;
using MathForge.Maui.Views;
using MathForge.Maui.Views.Common;
using MathForge.Maui.Views.Modules;
using MathForge.Maui.Views.Modules.Games;
using MathForge.Maui.Views.Modules.Graphics;
using MathForge.Maui.Views.Modules.Graphics.Drawings;
using MathForge.Maui.Views.Modules.Graphics.Fractals;
using MathForge.Maui.Views.Modules.Probability.Distributions;
using MathForge.Maui.Views.Modules.Utilities.Organizers;
using MathForge.Maui.Views.Modules.Utilities.Sensors;
using MathForge.Maui.Views.Modules.Utilities.Tools;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace MathForge.Maui.Systems.Application.Extensions;

public static class MauiExtensions
{
    extension(MauiAppBuilder builder)
    {
        public MauiAppBuilder RegisterServices()
        {
            builder.Logging.AddDebug();
            builder.Services.AddSerilog(ConfigureLogger, false, true);

            builder.UseLocalizationResourceManager(settings =>
            {
                settings.SupportNameWithDots();
                settings.AddResource(Components_Resources.ResourceManager);
                settings.AddResource(Common_Resources.ResourceManager);
                settings.AddResource(Distributions_Resources.ResourceManager);
                settings.AddResource(Measurement_Resources.ResourceManager);
                settings.AddResource(Parameters_Resources.ResourceManager);
                settings.RestoreLatestCulture(true);
                settings.SuppressTextNotFoundException();
            });


            builder.Services.AddSingleton<IMeasurementRegistry>(provider => provider.GetRequiredService<InMemoryMeasurementRegistry>());

            builder.Services.AddSingleton<IThemeService, ThemeService>();
            builder.Services.AddSingleton<ILocaleService, LocaleService>();
            builder.Services.AddSingleton<ActivityService>();

            builder.Services.AddSingleton<ParameterService>();
            builder.Services.AddSingleton<DistributionService>();

            builder.Services.AddSingleton<AccelerometerProcessingService>();
            builder.Services.AddSingleton<BarometerProcessingService>();

            builder.Services.AddSingleton<AccelerometerService>();
            builder.Services.AddSingleton<MagnetometerService>();
            builder.Services.AddSingleton<BarometerService>();
            builder.Services.AddSingleton<FlashlightService>();
            builder.Services.AddSingleton<RoundingService>();
            builder.Services.AddSingleton<CalculatorService>();

            return builder;
        }

        public MauiAppBuilder RegisterViewModels()
        {
            builder.Services.AddTransient(typeof(global::MathForge.Maui.ViewModels.Probability.Distributions.DistributionViewModel<>));

            return builder;
        }

        public MauiAppBuilder RegisterViewsWithViewModel()
        {
            // Light
            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<SettingsViewModel>();
            builder.Services.AddSingleton<SettingsPage>();
            builder.Services.AddSingleton<DistributionsViewModel>();
            builder.Services.AddSingleton<ProbabilityPage>();
            builder.Services.AddSingleton<NotesViewModel>();
            builder.Services.AddSingleton<NotesPage>();
            builder.Services.AddSingleton<ConverterViewModel>();
            builder.Services.AddSingleton<ConverterPage>();
            builder.Services.AddSingleton<CashCalculatorViewModel>();
            builder.Services.AddSingleton<CashCalculatorPage>();
            builder.Services.AddSingleton<CodeGeneratorViewModel>();
            builder.Services.AddSingleton<CodeGeneratorPage>();
            builder.Services.AddSingleton<SlidingTilePuzzleViewModel>();
            builder.Services.AddSingleton<SlidingTilePuzzlePage>();

            // Heavy
            builder.Services.AddTransient<FlashlightViewModel>();
            builder.Services.AddTransient<FlashlightPage>();
            builder.Services.AddTransient<CalculatorViewModel>();
            builder.Services.AddTransient<CalculatorPage>();
            builder.Services.AddTransient<LevelViewModel>();
            builder.Services.AddTransient<LevelPage>();
            builder.Services.AddTransient<StopwatchViewModel>();
            builder.Services.AddTransient<StopwatchPage>();
            builder.Services.AddTransient<TimerViewModel>();
            builder.Services.AddTransient<TimerPage>();
            builder.Services.AddTransient<AccelerometerViewModel>();
            builder.Services.AddTransient<AccelerometerPage>();
            builder.Services.AddTransient<BarometerViewModel>();
            builder.Services.AddTransient<BarometerPage>();
            builder.Services.AddTransient<SpeedometerViewModel>();
            builder.Services.AddTransient<SpeedometerPage>();
            builder.Services.AddTransient<CompassViewModel>();
            builder.Services.AddTransient<CompassPage>();
            builder.Services.AddTransient<CodeScannerViewModel>();
            builder.Services.AddTransient<CodeScannerPage>();
            builder.Services.AddTransient<MirrorViewModel>();
            builder.Services.AddTransient<MirrorPage>();
            builder.Services.AddTransient<SynthViewModel>();
            builder.Services.AddTransient<SynthPage>();
            builder.Services.AddTransient<SoundGeneratorViewModel>();
            builder.Services.AddTransient<SoundGeneratorPage>();
            builder.Services.AddTransient<UtilityViewModel>();
            builder.Services.AddTransient<UtilityPage>();
            builder.Services.AddTransient<DeviceInfoViewModel>();
            builder.Services.AddTransient<DeviceInfoPage>();

            return builder;
        }

        public MauiAppBuilder RegisterViews()
        {
            builder.Services.AddTransient<PreviewTableView>();
            builder.Services.AddTransient<PreviewSquareView>();

            return builder;
        }

        public MauiAppBuilder RegisterPages()
        {
            var lightPageTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t is { IsClass: true, IsAbstract: false } && typeof(ILightPage).IsAssignableFrom(t));

            foreach (var type in lightPageTypes)
                builder.Services.AddSingleton(type);

            // Heavy
            builder.Services.AddTransient<LineDrawPage>();
            builder.Services.AddTransient<CirclesDrawPage>();
            builder.Services.AddTransient<FillDrawPage>();
            builder.Services.AddTransient<TruncationDrawPage>();
            builder.Services.AddTransient<NoisesPage>();
            builder.Services.AddTransient<KochCurvePage>();

            // Conditional
            builder.Services.AddTransient<DistributionPage>();

            return builder;
        }

        public MauiAppBuilder ConfigureExceptions()
        {
            GlobalExceptionHandler.UnhandledException += GlobalExceptionHandler_UnhandledException;

            return builder;

            void GlobalExceptionHandler_UnhandledException(object sender, UnhandledExceptionEventArgs e)
            {
                var exception = e.ExceptionObject as Exception;

                Shell.Current.DisplayAlertAsync("Произошло неперехваченное исключение", exception?.Message, "Выход");

                Log.Error(exception, "Global exception caught");
                Debug.Print(exception?.Message);
                Console.WriteLine(exception?.Message);
            }
        }
    }

    private static void ConfigureLogger(LoggerConfiguration configuration)
    {
        const string outputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] [{Namespace}] [{Method}] {Message:lj}{NewLine}{Exception}";

        SelfLog.Enable(Console.WriteLine);

        configuration
            .MinimumLevel.Verbose()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Verbose)
            .Enrich.FromLogContext()
            .WriteTo.Debug(outputTemplate: outputTemplate)
            .WriteTo.Console(outputTemplate: outputTemplate, theme: AnsiConsoleTheme.Code);
    }
}
