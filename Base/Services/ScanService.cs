using BarcodeScanner.Mobile;
using Base.Enums;
using Base.Interfaces;
using Base.Models;
using Base.Static;
using Microsoft.Extensions.Logging;

namespace Base.Services
{
    public class ScanService : IScanService
    {
        private readonly IScanApiService _apiService;
        private readonly ICodeParseService _parseService;
        private readonly ILogger<ScanService> _logger;

        public ScanService(IScanApiService apiService, ICodeParseService parseService, ILogger<ScanService> logger)
        {
            _apiService = apiService;
            _parseService = parseService;
            _logger = logger;
        }

        public async Task<ScanElement> CreateScanElementAsync(BarcodeResult barcode, ScanChoose scanChoose, string guid, bool serverConnected, bool internetConnected)
        {
            var scanElement = new ScanElement();
            var scannedBarcode = barcode.DisplayValue;

            // Если код является Штрихкодом, добавляем информацию о коде, иначе об акцизе
            switch (scanChoose)
            {
                case ScanChoose.Штрихкод:
                    scanElement.Barcode = scannedBarcode;
                    break;
                case ScanChoose.Акциз:
                    scanElement.QRCodes.Add(scannedBarcode);
                    break;
            }

            // Если интернет или сервер 1С работают, получаем информацию
            if (serverConnected || internetConnected)
            {
                if (serverConnected)
                    scanElement = await _apiService.GetScannedCodeInformationAsync(barcode, scanChoose, guid);
                // Если есть корректный Штрихкод и Карточка не найдена в базе 1С, парсим сайт
                if (scanElement?.Barcode != "0000000000000" && scanElement?.Name == "Новая номенклатура")
                {
                    var name = await _parseService.FindNameByCode(scanElement.Barcode);
                    // Если название было найдено
                    if (name != "NULL")
                        scanElement.Name = name;
                }
            }

            return scanElement;
        }

        public bool IsCorrectChooseScannedCode(BarcodeResult code, ScanChoose method)
        {
            var format = code.BarcodeFormat;
            if (format.CompareBarcodeFormatToChooseFormat(method))
                return true;
            return false;
        }
    }
}
