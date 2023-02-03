using Base.Enums;
using Base.Interfaces;
using Base.Models;
using Base.Services;
using Base.ViewModels;
using Microsoft.Maui.Controls;
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

    private async void Delete_Clicked(object sender, EventArgs e)
    {
        var result = await DisplayAlert("Удалить", "Подтвердите удаление", "Да", "Нет");
        if(result == true)
        {
            var button = sender as Button;
            var element = button.BindingContext as ScanElement;
            var vm = BindingContext as ScannedCodesViewModel;
            vm.DeleteScanElement.Execute(element);
        }
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
        var result = await DisplayActionSheet("Выберите метод", "Отмена", null, Enum.GetNames(typeof(MethodsToSend)));
        ChangeLoading();
        try
        {
            if(result != "Отмена")
            {

                var scannedCodes = BindingContext as ScannedCodesViewModel;
                var response = await _apiService.SendScannedCodes(scannedCodes, Enum.Parse<MethodsToSend>(result));
                Dispatcher.Dispatch(async () => await DisplayAlert("Ответ от сервера:", response, "Спасибо!"));

            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            Dispatcher.Dispatch(async () => await DisplayAlert("Ошибка!", "При запросе информации с сервера, произошла ошибка, обратитесь к администратору", "Хорошо"));
        }
        ChangeLoading();
    }

    private void ChangeLoading()
    {
        ButtonClear.IsEnabled = !ButtonClear.IsEnabled;
        ButtonSend.IsEnabled = !ButtonSend.IsEnabled;
        ImageLoading.IsVisible = !ImageLoading.IsVisible;
        BoxViewLoading.IsVisible = !BoxViewLoading.IsVisible;
    }
}