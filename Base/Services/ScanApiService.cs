using BarcodeScanner.Mobile;
using Base.Enums;
using Base.Interfaces;
using Base.Models;
using Base.Static;
using Base.ViewModels;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Text.Unicode;

namespace Base.Services
{
    public class ScanApiService : IScanApiService
    {
        private HttpClient _httpClient;
        private JsonSerializerOptions _options;

        public ScanApiService()
        {
            _options = new JsonSerializerOptions()
            {
                NumberHandling = JsonNumberHandling.AllowReadingFromString |
                    JsonNumberHandling.WriteAsString,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            };
            SetupConnection();
        }

        public async Task<bool> TestConnectionToApiAsync()
        {
            SetupConnection();
            try
            {
                var jsonContent = new StringContent("{}", Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("get_request?ОбработатьШтрихкод=&УИД=", jsonContent);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {                
                return false;
            }

        }

        public async Task<ScanElement> GetScannedCodeInformationAsync(BarcodeResult barcode, ScanChoose scanChoose, string guid)
        {

            ScanElement scanElement = new();
            var barcodeFormat = barcode.BarcodeFormat;
            var scannedBarcode = barcode.DisplayValue;

            switch (scanChoose)
            {
                case ScanChoose.Штрихкод:
                    scanElement.Barcode = scannedBarcode;
                    break;
                case ScanChoose.Акциз:
                    scanElement.QRCodes.Add(scannedBarcode);
                    break;
            }

            var jsonContent = new StringContent("{}", Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("get_request?ОбработатьШтрихкод=" + scannedBarcode + "&УИД=" + guid, jsonContent);
            var content = await response.Content.ReadAsStringAsync();

            if(!response.IsSuccessStatusCode)
                throw new Exception(content.ToString());

            if (content == "false")
                return scanElement;

            try
            {
                var information = JsonSerializer.Deserialize<Element1CCodeInfo>(content, _options);
                scanElement = information.GetInformation(scanElement);
                return scanElement;
            }
            catch { throw new Exception(content.ToString()); }
        }

        public Task GetScannedCodesInformationAsync(ScannedCodesViewModel scannedCodes)
        {
            throw new NotImplementedException();
        }

        public async Task<string> SendScannedCodesAsync(ScannedCodesViewModel scannedCodes, MethodsToSend method)
        {
            SetupConnection();
            var model = new Element1CToSend();
            var codes = scannedCodes.ScannedCodes.ToList();
            foreach (var code in codes)
            {
                model.AddCardToList(code);
            }

            var jsonModel = JsonSerializer.Serialize<Element1CToSend>(model, _options);
            var jsonContent = new StringContent(jsonModel, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("get_request?ОбработатьСписокИзТСД=" + method.ToString(), jsonContent);
            var content = response.Content.ReadAsStringAsync();
            return await content;
        }

        public async Task<UserSettings> GetSettingsAsync(UserSettings settings)
        {
            Element1CSettings element1CSettings = null;
            SetupConnection();
            try
            {
                var jsonModel = "{\"Логин\":" + $"\"{settings.Login}\"" + ",\"Пароль\":" + $"\"{settings.Password}\"" + "}";
                var jsonContent = new StringContent(jsonModel, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"get_request?ПолучитьНастройки=&УИД={settings.Guid}", jsonContent);
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new Exception(content.ToString());

                element1CSettings = JsonSerializer.Deserialize<Element1CSettings>(content, _options);
                settings = element1CSettings.GetInformation(settings);
                return settings;
            }
            catch (Exception ex)
            {
                throw new Exception(element1CSettings?.Message ?? ex.Message);
            }
        }

        private void SetupConnection()
        {
#if ANDROID
            var connectonString = Preferences.Get(PreferencesProgram.ConnectionString, "localhost");
#endif
            _httpClient = new()
            {
                BaseAddress = new Uri($"http://194.187.150.246:45457/scbot/ru_RU/hs/sc_url/sc_EnterEmulate/"),
            };
            //_httpClient.Timeout = new TimeSpan(0,0,10);
        }

        private string CorrectCodeToSend (string code)
        {
            Regex regexFirst = new Regex("[\"]");
            Regex regexSecond = new Regex(@"[\\]");
            Regex regexThird = new Regex("[&]");
            code = regexFirst.Replace(code, "^^^");
            code = regexSecond.Replace(code, "***");
            code = regexThird.Replace(code, "%%%");
            return code;
        }
    }
}
