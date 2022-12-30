using Base.ViewModels;
using Microsoft.Maui.Controls.PlatformConfiguration;
using ZXing.Net.Maui.Controls;
using ZXing.Net.Maui;

namespace Base.Views;

public partial class ScanPage : ContentPage
{
    public ScanPageViewModel _scanModel { get; set; }
    //private static IViewHandler? handler {get;set;}
    public ScanPage()
	{
        InitializeComponent();
        ZXingCamera.Options = new BarcodeReaderOptions()
		{
			AutoRotate = true
		};
        _scanModel = Resources["scanText"] as ScanPageViewModel;
    }
    protected void BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        _scanModel.ScanText = e.Results[0].Value;
    }
    private void ToolbarItem_Clicked(object sender, EventArgs e)
	{
        ZXingCamera.IsTorchOn = !ZXingCamera.IsTorchOn;
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        //var temp = ZXingCamera;
        base.OnNavigatedTo(args);
    }
    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        //var temp = ZXingCamera;

#if ANDROID
        Android.Hardware.Camera.Open();
#endif

        base.OnNavigatedFrom(args);
    }
}