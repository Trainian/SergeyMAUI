using Base.Enums;
using Base.Interfaces;
using Base.Models;
using Base.Static;
using Base.ViewModels;
using System.Diagnostics;

namespace Base.Views;

public partial class ScannedList : ContentPage
{
    private readonly IScanApiService _apiService;

    public ScannedList(IScanApiService apiService)
    {
        _apiService = apiService;
        InitializeComponent();
    }

    private async void SwipeItem_Delete_Clicked(object sender, EventArgs e)
    {
        var result = await DisplayAlert("Удалить", "Подтвердите удаление", "Да", "Нет");
        if (result == true)
        {
            var item = sender as SwipeItem;
            var element = item.BindingContext as ScanElement;
            var vm = BindingContext as ScannedCodesViewModel;
            vm.DeleteScanElement.Execute(element);
        }
    }

    private async void SwipeItem_Edit_Clicked(object sender, EventArgs e)
    {
        var item = sender as SwipeItem;
        var element = item.BindingContext as ScanElement;

        await Navigation.PushAsync(new EditScannedElement()
        {
            BindingContext = element
        });
    }

    private async void ButtonClear_Clicked(object sender, EventArgs e)
    {
        var result = await DisplayAlert("Очистить", "Уверены что хотите очистить весь список?", "Да", "Нет");
        if (result == true)
        {
            var vm = BindingContext as ScannedCodesViewModel;
            vm.DeleteScannedCodes.Execute(null);
        }
    }

    private async void ButtonSend_Clicked(object sender, EventArgs e)
    {
        ChangeLoading();
        var connectedToApi = Preferences.Get(PreferencesProgram.ConnectedApi, false);
        if(connectedToApi == true)
        {
            var result = await DisplayActionSheet("Выберите метод", "Отмена", null, Enum.GetNames(typeof(MethodsToSend)));
            try
            {
                if (result != "Отмена")
                {
                    var scannedCodes = BindingContext as ScannedCodesViewModel;
                    var response = await _apiService.SendScannedCodesAsync(scannedCodes, Enum.Parse<MethodsToSend>(result));
                    Dispatcher.Dispatch(async () => await DisplayAlert("Ответ от сервера:", response, "Спасибо!"));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                Dispatcher.Dispatch(async () => await DisplayAlert("Ошибка!", "При запросе информации с сервера, произошла ошибка, обратитесь к администратору", "Хорошо"));
            }
        }
        else
        {
            Dispatcher.Dispatch(async () => await DisplayAlert("Нет соединения", "Отсутвует подключение к серверу, пожалуйста проверьте настройки соединения и переподключитесь на Главной странице, либо обратитесь к Администратору.", "Хорошо"));
        }
        ChangeLoading();
    }

    private void ButtonQRCodes_Clicked(object sender, EventArgs e)
    {
        var item = sender as ImageButton;
        var element = item.BindingContext as ScanElement;
        var isVisible = element.IsVisibleQRCodes = !element.IsVisibleQRCodes;
        if (isVisible)
            item.Source = "uparrow.svg";
        else
            item.Source = "downarrow.svg";        
    }
        
    private void ChangeLoading()
    {
        ButtonClear.IsEnabled = !ButtonClear.IsEnabled;
        ButtonSend.IsEnabled = !ButtonSend.IsEnabled;
        ccLoader.IsVisible = !ccLoader.IsVisible;
    }


}