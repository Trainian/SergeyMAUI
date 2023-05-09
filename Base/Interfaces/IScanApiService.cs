using BarcodeScanner.Mobile;
using Base.Enums;
using Base.Models;
using Base.ViewModels;

namespace Base.Interfaces
{
    public interface IScanApiService
    {
        Task<bool> TestConnectionToApiAsync();
        Task<string> SendScannedCodesAsync(ScannedCodesViewModel scannedCodes, MethodsToSend method);
        Task<ScanElement> GetScannedCodeInformationAsync(BarcodeResult barcode, ScanChoose scanChoose, string guid);
        Task GetScannedCodesInformationAsync(ScannedCodesViewModel scannedCodes);
        Task<UserSettings> GetSettingsAsync(UserSettings settings);
    }
}
