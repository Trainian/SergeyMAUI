using BarcodeScanner.Mobile;
using Base.Interfaces;
using Base.Services;
using Base.Views;
using Camera.MAUI;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace Base;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseMauiCameraView()
            .ConfigureMauiHandlers(e =>
            {
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
        builder.Services.AddSingleton<ScanPage2>();
        builder.Services.AddSingleton<SettingsPage>();
        builder.Services.AddTransient<IUserSettingsService, UserSettingsService>();
        builder.Services.AddTransient<IScanService, ScanService>();
        builder.Services.AddTransient<IScanApiService, ScanApiService>();
        builder.Services.AddTransient<ICodeParseService, CodeParseService>();

#if ANDROID
        builder.Services.AddTransient<ISpeechToText, Base.Platforms.SpeechToTextService>();
#endif

        return builder.Build();
    }
}
