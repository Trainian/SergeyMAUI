using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Platform.Android;
using Microsoft.Maui.Platform;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
using Android.App;
using Android.OS;
using Android.Runtime;

namespace Base;

[Application(UsesCleartextTraffic = true)]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
    }

    public override void OnCreate()
    {
        base.OnCreate();
    }


    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
