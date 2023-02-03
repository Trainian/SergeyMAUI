using Base.Interfaces;
using Base.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Services
{
    public class UserSettingsService : IUserSettingsService
    {
        public Task<UserSettings> GetUserSettings()
        {
            if (Preferences.Get("Login", "") == "")
                return Task.FromResult(new UserSettings());

            var userSettings = new UserSettings();
            userSettings.Login = Preferences.Get("Login", "NotSet");
            userSettings.Password = Preferences.Get("Password", "NotSet");
            userSettings.Guid = Preferences.Get("Guid", "NotSet");
            userSettings.ConnectionString = Preferences.Get("ConnectionString", "NotSet");
            return Task.FromResult(userSettings);
        }

        public Task<bool> SetUserSettings(UserSettings userSettings)
        {
            try
            {
                Preferences.Set("Login", userSettings.Login);
                Preferences.Set("Password", userSettings.Password);
                Preferences.Set("Guid", userSettings.Guid);
                Preferences.Set("ConnectionString", userSettings.ConnectionString);
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка записи настроек {ex.Message}");
                return Task.FromResult(false);
            }
        }
    }
}
