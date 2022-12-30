using Base.Interfaces;
using Base.Platforms;
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
				e.AddHandler(typeof(CameraView), typeof(CameraViewHandler));
				e.AddHandler(typeof(BarcodeGeneratorView), typeof(BarcodeGeneratorViewHandler));
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
		builder.Services.AddTransient<ISpeechToText, SpeechToTextService>();

		return builder.Build();
	}
}
