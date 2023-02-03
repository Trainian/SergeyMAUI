using Base.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Interfaces
{
    public interface IUserSettingsService
    {
        Task<UserSettings> GetUserSettings();
        Task<bool> SetUserSettings(UserSettings userSettings);
    }
}
