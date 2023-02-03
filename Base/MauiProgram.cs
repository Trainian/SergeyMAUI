using BarcodeScanner.Mobile.Maui;
using Base.Interfaces;
using Base.Platforms;
using Base.Services;
using Base.Views;
using Microsoft.Extensions.Logging;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;

namespace Base;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseBarcodeReader()
            .ConfigureMauiHandlers(e =>
			{
				e.AddHandler(typeof(CameraBarcodeReaderView), typeof(CameraBarcodeReaderViewHandler));
				e.AddHandler(typeof(ZXing.Net.Maui.Controls.CameraView), typeof(ZXing.Net.Maui.CameraViewHandler));
				e.AddHandler(typeof(BarcodeGeneratorView), typeof(BarcodeGeneratorViewHandler));
                e.AddBarcodeScannerHandler();
            })
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<SpeechPage>();
        builder.Services.AddSingleton<ScanPage>();
        builder.Services.AddSingleton<SettingsPage>();
		builder.Services.AddTransient<ISpeechToText, SpeechToTextService>();
		builder.Services.AddTransient<IUserSettingsService, UserSettingsService>();
		builder.Services.AddTransient<IScanApiService, ScanApiService>();

		return builder.Build();
	}
}
