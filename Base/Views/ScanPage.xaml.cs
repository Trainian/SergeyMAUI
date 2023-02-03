using Base.ViewModels;
using Microsoft.Maui.Controls.PlatformConfiguration;
using static Microsoft.Maui.ApplicationModel.Permissions;
using Base.Models;
using Base.Interfaces;
using Base.Enums;
using BarcodeScanner.Mobile.Core;
using Android.Content;
using Android.Hardware.Camera2;
using System.Diagnostics;

namespace Base.Views;

public partial class ScanPage : ContentPage
{
    private IScanApiService _apiService;
    public ScanPageViewModel scanModel { get; set; }
    public ScannedCodesViewModel scannedCodes { get; set; }
    //private static IViewHandler? handler {get;set;}
    public ScanPage(IScanApiService apiService)
	{
        InitializeComponent();
        scanModel = Resources["scanText"] as ScanPageViewModel;
        scannedCodes = new ScannedCodesViewModel();
        _apiService = apiService;
        BarcodeScanner.Mobile.Core.Methods.SetSupportBarcodeFormat(BarcodeFormats.QRCode | BarcodeFormats.Ean13 | BarcodeFormats.DataMatrix);
        TestData();
    }

    private async void CameraView_OnDetected(object sender, BarcodeScanner.Mobile.Core.OnDetectedEventArg e)
    {
        var code = scanModel.ScanText = e.BarcodeResults[0].DisplayValue;
        var element = new ScanElement() { ScannedCode = code };
        Dispatcher.Dispatch(() => imgVerification.IsVisible = true);

        var connect = Connectivity.Current.NetworkAccess;
        if (connect == NetworkAccess.Internet)
        {
            await GetInfo(element);
        }

        await Task.Delay(1000);
        this.Dispatcher.Dispatch(() =>
        {
            imgVerification.IsVisible = false;
            Scaner.IsScanning = true;
        });
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
	{
        Scaner.TorchOn = !Scaner.TorchOn;
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);

        // Перезапускаем поток камеры
        Scaner.CameraFacing = CameraFacing.Front;
        Scaner.CameraFacing = CameraFacing.Back;
    }

    private async void ButtonList_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ScannedList(_apiService)
        {
            BindingContext = scannedCodes
        });
    }

    private async void ButtonSend_Clicked(object sender, EventArgs e)
    {
        var result = await DisplayActionSheet("Выберите метод", "Отмена", null, Enum.GetNames(typeof(MethodsToSend)));
        try
        {

            var response = await _apiService.SendScannedCodes(scannedCodes, Enum.Parse<MethodsToSend>(result));
            Dispatcher.Dispatch(async () => await DisplayAlert("Ответ от сервера:", response, "Спасибо!"));
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            Dispatcher.Dispatch(async () => await DisplayAlert("Ошибка!", "При запросе информации с сервера, произошла ошибка, обратитесь к администратору", "Хорошо"));
        }        
    }
    
    private async void ButtonEdit_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new EditScannedElement()
        {
            BindingContext = scannedCodes.ScannedCodes.Last()
        });
    }

    private async Task GetInfo(ScanElement element)
    {
        try
        {
            element = await _apiService.GetScannedCodeInformation(element);
            scanModel.ScanText += "\n" + element.Name;
            scanModel.ScanText += "\n остаток: " + element.QuantityLeft + "шт., цена: " + element.Price + "руб.";
        }
        catch (Exception)
        {
            Dispatcher.Dispatch(async () => await DisplayAlert("Ошибка!", "При запросе информации с сервера, произошла ошибка, обратитесь к администратору", "Хорошо"));
        }

        scannedCodes.AddScannedCode(element);
    }
    private void TestData()
    {
        for (int i = 1; i < 9; i++)
        {
            scannedCodes.AddScannedCode(new ScanElement()
            {
                ScannedCode = $"123456789012{i}",
                CountChoise = i
            });
        }
    }
}