using Base.Interfaces;
using Base.Models;
using Base.Static;
using System.Diagnostics;

namespace Base.Services
{
    public class UserSettingsService : IUserSettingsService
    {
        public Task<UserSettings> GetUserSettingsAsync()
        {
            if (Preferences.Get(PreferencesProgram.Login, "") == "")
                return Task.FromResult(new UserSettings());

            var userSettings = new UserSettings();
            userSettings.Login = Preferences.Get(PreferencesProgram.Login, "");
            userSettings.Password = Preferences.Get(PreferencesProgram.Password, "");
            userSettings.Guid = Preferences.Get(PreferencesProgram.Guid, "");
            userSettings.PayDate = Preferences.Get(PreferencesProgram.PayDate, "");
            userSettings.ConnectionString = Preferences.Get(PreferencesProgram.ConnectionString, "");
            userSettings.Organization = Preferences.Get(PreferencesProgram.Organization, "");
            userSettings.Warehouse = Preferences.Get(PreferencesProgram.Warehouse, "");
            userSettings.TypeOfPrices = Preferences.Get(PreferencesProgram.TypeOfPrices, "");
            return Task.FromResult(userSettings);
        }

        public Task<bool> SaveUserSettingsAsync(UserSettings userSettings)
        {
            try
            {
                Preferences.Set(PreferencesProgram.Login, userSettings.Login);
                Preferences.Set(PreferencesProgram.Password, userSettings.Password);
                Preferences.Set(PreferencesProgram.Guid, userSettings.Guid);
                Preferences.Set(PreferencesProgram.PayDate, userSettings.PayDate);
                Preferences.Set(PreferencesProgram.ConnectionString, userSettings.ConnectionString);
                Preferences.Set(PreferencesProgram.Organization, userSettings.Organization);
                Preferences.Set(PreferencesProgram.Warehouse, userSettings.Warehouse);
                Preferences.Set(PreferencesProgram.TypeOfPrices, userSettings.TypeOfPrices);
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка записи настроек: {ex.Message}");
                return Task.FromResult(false);
            }
        }
    }
}
