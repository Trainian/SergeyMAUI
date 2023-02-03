using Base.Interfaces;
using Base.Models;

namespace Base.Views;

public partial class SettingsPage : ContentPage
{
    private readonly IUserSettingsService _userService;

    public SettingsPage(IUserSettingsService userService)
	{
        _userService = userService;
		InitializeComponent();
        SetUserSettings();
	}

    private async void ChooseSettings_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("Разработка", "Метод находится в разработке", "Ждем");
    }

    private async void Save_Clicked(object sender, EventArgs e)
    {
        if(!VerifySettings())
        {
            await DisplayAlert("Ошибка", "Заполните обязательные поля: Логин, Пароль, Строка подключения.", "Хорошо");
            return;
        }
        var userSettings = CreateUserSettings();
        await _userService.SetUserSettings(userSettings);
        await DisplayAlert("Успех", "Данные успешно сохранены!", "Хорошо");
    }

    private UserSettings CreateUserSettings ()
    {
        var userSettings = new UserSettings();
        userSettings.Login = Login.Text.Trim();
        userSettings.Password = Password.Text.Trim();
        userSettings.ConnectionString = ConnectionString.Text.Trim();
        return userSettings;
    }

    private async void SetUserSettings()
    {
        var userSettings = await _userService.GetUserSettings();
        Login.Text = userSettings.Login;
        Password.Text = userSettings.Password;
        ConnectionString.Text = userSettings.ConnectionString;
        PayDate.Text = userSettings.PayDate.ToString();
        Guid.Text = userSettings.Guid;
    }

    private bool VerifySettings()
    {
        var login = Login.Text ?? "";
        var password = Password.Text ?? "";
        var connection = ConnectionString.Text ?? "" ;
        if(string.IsNullOrWhiteSpace(login) ||
            string.IsNullOrWhiteSpace(password) ||
            string.IsNullOrWhiteSpace(connection))
        {
            return false;
        }
        return true;
    }
}