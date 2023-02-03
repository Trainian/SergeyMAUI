using Base.Interfaces;
using Base.Models;
using Base.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net;
using System.Text.Json.Serialization;
using Base.Enums;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Text.Json.Nodes;

namespace Base.Services
{
    public class ScanApiService : IScanApiService
    {
        private HttpClient _httpClient;
        private JsonSerializerOptions _options;

        public ScanApiService()
        {
            _httpClient = new()
            {
                BaseAddress = new Uri("http://193.176.157.27:45497/scbot/ru_RU/hs/sc_url/sc_EnterEmulate/"),
            };

            _options = new JsonSerializerOptions()
            {
                NumberHandling = JsonNumberHandling.AllowReadingFromString |
                    JsonNumberHandling.WriteAsString,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            };
        }

        public async Task<ScanElement> GetScannedCodeInformation(ScanElement scannedCode)
        {
            var jsonContent = new StringContent("{}", Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("get_request?ОбработатьШтрихкод=" + scannedCode.ScannedCode, jsonContent);
            var content = await response.Content.ReadAsStringAsync();

            if (content == "false")
                return scannedCode;

            var information = JsonSerializer.Deserialize<Root>(content, _options);
            scannedCode.Name = information.nom_info.First().Владелец;
            scannedCode.QuantityLeft = information.nom_info.First().КоличествоОстаток;
            scannedCode.Price = information.nom_info.First().Цена;
            return scannedCode;
        }

        public Task GetScannedCodesInformation(ScannedCodesViewModel scannedCodes)
        {
            throw new NotImplementedException();
        }

        public async Task<string> SendScannedCodes(ScannedCodesViewModel scannedCodes, MethodsToSend method)
        {
            var model = new RootSend();
            var codes = scannedCodes.ScannedCodes.ToList();
            foreach (var code in codes)
            {
                model.list_info.Add(new List_info()
                {
                    Штрихкод = code.ScannedCode,
                    Количество = code.CountChoise,
                    Цена = code.Price
                });
            }

            var jsonModel = JsonSerializer.Serialize<RootSend>(model, _options);
            var jsonContent = new StringContent(jsonModel, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("get_request?ОбработатьСписокИзТСД=" + method.ToString(), jsonContent);
            var content = response.Content.ReadAsStringAsync();
            return await content;
        }

        private class Root
        {
            public List<Nom_info> nom_info { get; set; } = new List<Nom_info>();
        }

        private class Nom_info
        {
            public string Владелец { get; set; }
            public int КоличествоОстаток { get; set; }
            public decimal Цена { get; set; }
        }

        private class RootSend
        {
            public List<List_info> list_info { get; set; } = new List<List_info>();
        }

        public class List_info
        {
            public string Штрихкод { get; set; }
            public int Количество { get; set; }
            public decimal Цена { get; set; }
        }
    }
}
