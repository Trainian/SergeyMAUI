using BarcodeScanner.Mobile;
using Base.CustomControls;
using Base.Enums;
using Base.Interfaces;
using Base.Models;
using Base.Static;
using Base.ViewModels;
using Camera.MAUI;
using CommunityToolkit.Maui.Alerts;
using System.Diagnostics;
using System.Text;

namespace Base.Views;

public partial class ScanPage2 : ContentPage
{
    private IScanService _scanService;
    private IScanApiService _scanApiService;
    public ScanPageViewModel scanModel { get; set; }
    public ScannedCodesViewModel scannedCodes { get; set; }
    public ScanPage2(IScanService scanService, IScanApiService scanApiService)
    {
        InitializeComponent();
        scanModel = Resources["scanPage"] as ScanPageViewModel;
        scannedCodes = new ScannedCodesViewModel();
        _scanService = scanService;
        _scanApiService = scanApiService;
        cameraView.CamerasLoaded += CameraView_CamerasLoaded;
        cameraView.BarcodeDetected += CameraView_BarcodeDetected;
        cameraView.BarCodeDetectionFrameRate = 5;
        cameraView.BarCodeOptions = new Camera.MAUI.ZXingHelper.BarcodeDecodeOptions
        {
            AutoRotate = true,
            ReadMultipleCodes = false,
            TryHarder = true,
            TryInverted = false,
            //PossibleFormats = { 
            //    ZXing.BarcodeFormat.All_1D,
            //    ZXing.BarcodeFormat.AZTEC,
            //    ZXing.BarcodeFormat.CODABAR,
            //    ZXing.BarcodeFormat.CODE_128,
            //    ZXing.BarcodeFormat.CODE_39,
            //    ZXing.BarcodeFormat.CODE_93,
            //    ZXing.BarcodeFormat.DATA_MATRIX,
            //    ZXing.BarcodeFormat.EAN_13,
            //    ZXing.BarcodeFormat.EAN_8,
            //    ZXing.BarcodeFormat.IMB,
            //    ZXing.BarcodeFormat.ITF,
            //    ZXing.BarcodeFormat.MAXICODE,
            //    ZXing.BarcodeFormat.MSI,
            //    ZXing.BarcodeFormat.PDF_417,
            //    ZXing.BarcodeFormat.PHARMA_CODE,
            //    ZXing.BarcodeFormat.PLESSEY,
            //    ZXing.BarcodeFormat.QR_CODE,
            //    ZXing.BarcodeFormat.RSS_14,
            //    ZXing.BarcodeFormat.RSS_EXPANDED,
            //    ZXing.BarcodeFormat.UPC_A,
            //    ZXing.BarcodeFormat.UPC_E,
            //    ZXing.BarcodeFormat.UPC_EAN_EXTENSION}
            PossibleFormats = { ZXing.BarcodeFormat.DATA_MATRIX, ZXing.BarcodeFormat.All_1D }
        };
    }


    private async void CameraView_BarcodeDetected(object sender, Camera.MAUI.ZXingHelper.BarcodeEventArgs args)
    {
        var toast = Toast.Make($"Результат: {args.Result[0].Text}.");
        Dispatcher.Dispatch(async () => await toast.Show());
        
        Debug.WriteLine("BarcodeText=" + args.Result[0].Text);
    }

    private void CameraView_CamerasLoaded(object sender, EventArgs e)
    {
        cameraPicker.ItemsSource = cameraView.Cameras;
        cameraPicker.SelectedIndex = 0;
    }

    private async void OnStartClicked(object sender, EventArgs e)
    {
        if (cameraPicker.SelectedItem != null && cameraPicker.SelectedItem is CameraInfo camera)
        {
            cameraLabel.BackgroundColor = Colors.White;
            cameraView.Camera = camera;
            cameraView.BarCodeDetectionEnabled = true;
            var result = await cameraView.StartCameraAsync();
            Debug.WriteLine("Start camera result " + result);
        }
        else
        {
            cameraLabel.BackgroundColor = Colors.Red;
        }
    }
    private async void OnStopClicked(object sender, EventArgs e)
    {
        var result = await cameraView.StopCameraAsync();
        Debug.WriteLine("Stop camera result " + result);
    }
    private void OnSnapClicked(object sender, EventArgs e)
    {
        var result = cameraView.GetSnapShot(Camera.MAUI.ImageFormat.PNG);
        if (result != null)
            snapPreview.Source = result;
    }

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        cameraView.MirroredImage = e.Value;
    }
    private void CheckBox4_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        cameraView.TorchEnabled = e.Value;
    }
    private void CheckBox3_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        cameraView.BarCodeDetectionEnabled = e.Value;
    }

    private void Stepper_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        if (cameraView != null) cameraView.ZoomFactor = (float)e.NewValue;
    }

    private void cameraPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cameraPicker.SelectedItem != null && cameraPicker.SelectedItem is CameraInfo camera)
        {
            torchLabel.IsEnabled = torchCheck.IsEnabled = camera.HasFlashUnit;
            if (camera.MaxZoomFactor > 1)
            {
                zoomLabel.IsEnabled = zoomStepper.IsEnabled = true;
                zoomStepper.Maximum = camera.MaxZoomFactor;
            }
            else
                zoomLabel.IsEnabled = zoomStepper.IsEnabled = true;
        }
    }
}