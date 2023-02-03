using Base.ViewModels;
using Microsoft.Maui.Controls.PlatformConfiguration;
using ZXing.Net.Maui.Controls;
using ZXing.Net.Maui;
using static Microsoft.Maui.ApplicationModel.Permissions;
using Base.Models;
using Base.Interfaces;
using Base.Enums;

namespace Base.Views;

public partial class ScanZXingPage : ContentPage
{
    private IScanApiService _apiService;
    public ScanPageViewModel scanModel { get; set; }
    public ScannedCodesViewModel scannedCodes { get; set; }
    //private static IViewHandler? handler {get;set;}
    public ScanZXingPage(IScanApiService apiService)
	{
        InitializeComponent();
        ZXingCamera.Options = new BarcodeReaderOptions()
		{
			AutoRotate = true
		};
        scanModel = Resources["scanText"] as ScanPageViewModel;
        scannedCodes = new ScannedCodesViewModel();
        _apiService = apiService;
    }
    protected async void BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        var element = new ScanElement() { ScannedCode = e.Results[0].Value };

        ZXingCamera.IsDetecting = false;
        Dispatcher.Dispatch(() => imgVerification.IsVisible = true);

        scanModel.ScanText = element.ScannedCode;
        await GetInfo(element).ConfigureAwait(false);

        Dispatcher.Dispatch(() => imgVerification.IsVisible = false);
        await Task.Delay(2000);
        ZXingCamera.IsDetecting = true;
    }

    private async Task GetInfo(ScanElement element)
    {
        try
        {
            element = await _apiService.GetScannedCodeInformation(element);
            scanModel.ScanText += "\n"+element.Name;
            scanModel.ScanText += "\n остаток: " + element.QuantityLeft + "шт., цена: " + element.Price + "руб.";
        }
        catch (Exception)
        {
            Dispatcher.Dispatch(async () => await DisplayAlert("Ошибка!", "При запросе информации с сервера, произошла ошибка, проверьте что у вас работает Интернет", "Хорошо"));
        }

        scannedCodes.AddScannedCode(element);
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
	{
        ZXingCamera.IsTorchOn = !ZXingCamera.IsTorchOn;
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
#if ANDROID
        Android.Hardware.Camera.Open();
#endif

        base.OnNavigatedFrom(args);
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
        await DisplayAlert("Был выбран метод:", result, "Спасибо!");
    }
    
    private async void ButtonInfrormation_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("Разработка", "Метод находится в разработке", "Ждем");
    }
}