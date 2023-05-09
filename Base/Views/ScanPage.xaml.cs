using BarcodeScanner.Mobile;
using Base.CustomControls;
using Base.Enums;
using Base.Interfaces;
using Base.Models;
using Base.Static;
using Base.ViewModels;
using Camera.MAUI;
using CommunityToolkit.Maui.Alerts;
using System.Text;

namespace Base.Views;

public partial class ScanPage : ContentPage
{
    private IScanService _scanService;
    private IScanApiService _scanApiService;
    private IUserSettingsService _userSettings;
    private bool _internetConnected;
    private bool _serverConnected;
    private string _guid;
    public ScanPageViewModel scanModel { get; set; }
    public ScannedCodesViewModel scannedCodes { get; set; }
    public ScanPage(IScanService scanService, IScanApiService scanApiService, IUserSettingsService userSettings)
    {
        InitializeComponent();
        scanModel = Resources["scanPage"] as ScanPageViewModel;
        scannedCodes = new ScannedCodesViewModel();
        _scanService = scanService;
        _scanApiService = scanApiService;
        _userSettings = userSettings;
        SetupSettings();
        //TestData();
    }

    private async void SetupSettings()
    {
        var settings = await _userSettings.GetUserSettingsAsync();
        _guid = settings.Guid;
#if ANDROID
        BarcodeScanner.Mobile.Methods.SetSupportBarcodeFormat(BarcodeFormats.QRCode |
                                                        BarcodeFormats.DataMatrix |
                                                        BarcodeFormats.Ean8 |
                                                        BarcodeFormats.Ean13 |
                                                        BarcodeFormats.Code128 |
                                                        BarcodeFormats.Code39 |
                                                        BarcodeFormats.Code93);
#endif
    }

    private async void CameraView_OnDetected(object sender, BarcodeScanner.Mobile.OnDetectedEventArg e)
    {
        var isDuplicate = false;
        var correctScanMethod = _scanService.IsCorrectChooseScannedCode(e.BarcodeResults[0], scanModel.ScanChoose);

        //Если вид сканирования не верный, возвращаем уведомление
        if (!correctScanMethod)
        {
            var toast = Toast.Make($"Ожидаемый метод сканирования: {Enum.GetName(typeof(ScanChoose), scanModel.ScanChoose)}.");
            await toast.Show();
            await Task.Delay(1000);
            this.Dispatcher.Dispatch(() => Scaner.IsScanning = true);
            return;
        }

        Dispatcher.Dispatch(() => imgVerification.IsVisible = true);

        // Проверяем если это Акциз, был ли он ранее отсканирован
        if(scanModel.ScanChoose == ScanChoose.Акциз)
            isDuplicate = scannedCodes.SearchScannedCode(e.BarcodeResults[0], scanModel.ScanChoose) != null ? true : false;

        if (isDuplicate)
        {
            var toast = Toast.Make($"Данный код уже был отсканирован ранее.");
            await toast.Show();
        }
        else
        {
            // Получаем ScanElement
            scanModel.ScanElement = await GetScanElement(e.BarcodeResults[0]);

        }

        await Task.Delay(1000);
        Dispatcher.Dispatch(() => imgVerification.IsVisible = false);
        this.Dispatcher.Dispatch(() => { Scaner.IsScanning = true; });

    }

    /// <summary>
    /// Переключение фонарика
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Flashlight_Clicked(object sender, EventArgs e)
    {
        Scaner.TorchOn = !Scaner.TorchOn;
    }

    private void ScanFormat_Clicked(object sender, EventArgs e)
    {        
        //var popup = new MessagePopup("Сообщение", 0);
        //this.ShowPopup(popup);
    }

    /// <summary>
    /// Открыть список отсканированных элементов
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void ButtonList_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ScannedList(_scanApiService)
        {
            BindingContext = scannedCodes
        });
    }

    /// <summary>
    /// Открыть редактирование отсканированного элемента
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void ButtonEdit_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new EditScannedElement()
        {
            BindingContext = scanModel.ScanElement
        });
    }

    /// <summary>
    /// Переключение между форматами
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SwitchTo_Clicked(object sender, EventArgs e)
    {
        var imgBtn = sender as ImageButton;
        var switchTo = imgBtn.StyleId;
        switch (switchTo)
        {
            case "QRCode":
                scanModel.ScanChoose = ScanChoose.Акциз;
                break;

            case "Barcode":
                scanModel.ScanChoose = ScanChoose.Штрихкод;
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Создаем или получаем ScanElement из списка отсканированных
    /// </summary>
    /// <param name="barcode">Отсканированный код</param>
    /// <returns>Отсканированный элемент, при ошибке возвращает пустой экземпляр и Alert</returns>
    private async Task<ScanElement> GetScanElement(BarcodeResult barcode)
    {
        CheckConnections();
        ScanElement scanElement = null;
        try
        {
            scanElement = scannedCodes.SearchScannedCode(barcode, scanModel.ScanChoose);
            if (scanElement == null)
                scanElement = await _scanService.CreateScanElementAsync(barcode, scanModel.ScanChoose, _guid, _serverConnected, _internetConnected);

            return scannedCodes.AddOrUpdateScannedCode(scanElement, scanModel.ScanChoose);
        }
        catch (Exception e)
        {
            Dispatcher.Dispatch(async () => await DisplayAlert("Ошибка!", e.Message, "Хорошо"));
            return scanElement;
        }
    }

    /// <summary>
    /// Проверяем есть ли соединение с интернетом и сервером 1С
    /// </summary>
    private void CheckConnections()
    {
        _internetConnected = Connectivity.Current.NetworkAccess == NetworkAccess.Internet ? true : false;
        if (_internetConnected)
            _serverConnected = Preferences.Get(PreferencesProgram.ConnectedApi, false);
    }

    //private void TestData()
    //{
    //    for (int i = 1; i < 9; i++)
    //    {
    //        scannedCodes.AddOrUpdateScannedCode(new ScanElement()
    //        {
    //            Barcode = $"123456789012{i}",
    //            CountScanned = i
    //        });
    //    }
    //    var element = scannedCodes.ScannedCodes.First();
    //    for(int i = 1; i < 5; i++)
    //    {
    //        var random = new Random();
    //        var length = 128;
    //        string chars = "0123456789abcdefghijklmnopqrstuvwxyz";
    //        StringBuilder builder = new StringBuilder(length);

    //        for (int j = 0; j < length; ++j)
    //            builder.Append(chars[random.Next(chars.Length)]);

    //        element.QRCodes.Add(builder.ToString());
    //    }
    //}

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
#if ANDROID
        // Запрашиваем и проверяем доступ к камере
        bool allowed = await BarcodeScanner.Mobile.Methods.AskForRequiredPermission();
#endif
        // Перезапускаем поток камеры
        Scaner.CameraFacing = CameraFacing.Front;
        Scaner.CameraFacing = CameraFacing.Back;
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);

        // При переходе на новую вкладку, выключаем фонарик
        Scaner.TorchOn = false;
    }

}