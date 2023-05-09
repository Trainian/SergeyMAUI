using Base.Interfaces;
using Base.Models;
using Base.Static;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Maui.Core;
using System.Globalization;
using Base.ViewModels;

namespace Base.Views;

public partial class SettingsPage : ContentPage
{
    private readonly IUserSettingsService _userService;
    private readonly IScanApiService _apiService;
    public SettingsPageViewModel settingsModel;

    public SettingsPage(IUserSettingsService userService, IScanApiService apiService)
    {
        InitializeComponent();
        _userService = userService;
        _apiService = apiService;
        settingsModel = Resources["settingsPage"] as SettingsPageViewModel;
        FillUserSettings();
    }

    private async void FillUserSettings()
    {
        var userSettings = await _userService.GetUserSettingsAsync();
        settingsModel.SetSettings(userSettings);
    }

    private async void VerifySettings_Clicked(object sender, EventArgs e)
    {
        var connected = await ConnectionTest();
        var isDataFilling = VerifySettingsToFilling();
        var userSettings = settingsModel.GetUserSettings();
        if (connected && isDataFilling)
        {
            try {
                userSettings = await _apiService.GetSettingsAsync(userSettings);
                settingsModel.SetSettings(userSettings);
                await _userService.SaveUserSettingsAsync(userSettings);

                var toast = Toast.Make("Успех", ToastDuration.Long);
                await toast.Show();
            }
            catch (Exception ex)
            {
                await Dispatcher.DispatchAsync(() => DisplayAlert("Ошибка", ex.Message, "Хорошо"));
            }
        }
    }

    private async void Save_Clicked(object sender, EventArgs e)
    {
        if (!VerifySettingsToFilling())
        {
            await DisplayAlert("Ошибка", "Заполните обязательные поля, отмеченные *", "Хорошо");
            return;
        }
        var userSettings = settingsModel.GetUserSettings();
        await _userService.SaveUserSettingsAsync(userSettings);
        await DisplayAlert("Успех", "Данные успешно сохранены!", "Хорошо");
    }

    private bool VerifySettingsToFilling()
    {
        var login = Login.Text.Trim() ?? "";
        var password = Password.Text.Trim() ?? "";
        var guid = Guid.Text.Trim() ?? "";
        var connectionString = ConnectionString.Text.Trim() ?? "";
        if (string.IsNullOrWhiteSpace(login) ||
            string.IsNullOrWhiteSpace(password))
        {
            return false;
        }
        return true;
    }

    private async Task<bool> ConnectionTest()
    {
        var result = false;
        ccLoadControl.IsVisible = true;
        Preferences.Set(PreferencesProgram.ConnectedApi, false);
        var connection = await _apiService.TestConnectionToApiAsync();
        if (connection)
        {
            var toast = Toast.Make("Соединение установлено!");
            await toast.Show();
            Preferences.Set(PreferencesProgram.ConnectedApi, true);
            result = true;
        }
        else
        {
            var toast = Toast.Make("Проблемы с установкой соединения!");
            await toast.Show();
        }
        ccLoadControl.IsVisible = false;
        return result;
    }

}