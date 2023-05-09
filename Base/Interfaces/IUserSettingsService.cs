using Base.Models;

namespace Base.Interfaces
{
    public interface IUserSettingsService
    {
        Task<UserSettings> GetUserSettingsAsync();
        Task<bool> SaveUserSettingsAsync(UserSettings userSettings);
    }
}
