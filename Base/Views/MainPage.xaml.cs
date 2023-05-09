using Base.Static;
using Base.Interfaces;
using Base.Models;

namespace Base.Views;

public partial class MainPage : ContentPage
{
    private readonly IScanApiService _apiService;
    private readonly IUserSettingsService _userService;

    public MainPage(IScanApiService apiService, IUserSettingsService userService)
    {
        InitializeComponent();
        _apiService = apiService;
        _userService = userService;
        Button_Reconnection_Clicked(this, null);
    }

    private async void Button_Reconnection_Clicked(object sender, EventArgs e)
    {
        var settingsVerify = await CheckSetupSettings();
        if (settingsVerify)
        {
            ConnectionTest();
        }
        else
        {
            lblConnectionText.Text = "<b>Необходима настройка !</b><br>Для дальнейшей работы приложения, пожалуйста, перейдите в настройки и заполните их либо зарегистрируйтесь.";
        }
    }
    private async Task<bool> CheckSetupSettings()
    {
        var userSettings = await _userService.GetUserSettingsAsync();
        return VerifySettingsToFilling(userSettings);
    }

    private async void ConnectionTest ()
    {
        ccLoadControl.IsVisible = true;
        Preferences.Set(PreferencesProgram.ConnectedApi, false);
        var connection = await _apiService.TestConnectionToApiAsync();
        if (connection)
        {
            lblConnectionText.Text = "<b>Соединение успешно установлено!</b>\nМожно работать";
            Preferences.Set(PreferencesProgram.ConnectedApi, true);
        }
        else
            lblConnectionText.Text = "<b>Проблемы с установкой соединения!</b>\nПопробуйте переподключиться или обратитесь к администратору.";
        ccLoadControl.IsVisible = false;
    }

    private bool VerifySettingsToFilling(UserSettings settings)
    {
        if (string.IsNullOrWhiteSpace(settings.Login) ||
            string.IsNullOrWhiteSpace(settings.Password) ||
            string.IsNullOrWhiteSpace(settings.Guid))
        {
            return false;
        }
        return true;
    }
}

