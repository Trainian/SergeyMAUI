using BarcodeScanner.Mobile;
using Base.Enums;
using Base.Interfaces;
using Base.Models;
using Base.Services;
using Moq;

namespace Base.Tests.Services
{
    public class ScanServiceTests
    {
        Base.Services.ScanService _service;
        public ScanServiceTests()
        {
            _service = new ScanService(null, null, null);
        }

        [Theory]
        [InlineData("1234567890123")]
        [InlineData("1234561230123")]
        [InlineData("1234567890100")]
        public async Task CreateScanElement_WithoutConnections_InputBarcodeAndZeroQR(string code)
        {
            var barcode = new BarcodeResult() { DisplayValue = code, RawValue = code, BarcodeFormat = BarcodeFormats.Ean13 };

            var result = await _service.CreateScanElementAsync(barcode, ScanChoose.Штрихкод, "", false, false);

            Assert.Equal(code, result.Barcode);
            Assert.Equal(0, result.QRCodesCount);
        }

        [Fact]
        public async Task CreateScanElement_WithInternetConnectionAndWithoutServerConnection_CorrectName()
        {
            var name = "Новое название";
            var barcode = new BarcodeResult() { DisplayValue = "0", RawValue = "0", BarcodeFormat = BarcodeFormats.Ean13 };
            var mockParse = new Mock<ICodeParseService>();
            mockParse.Setup(m => m.FindNameByCode(barcode.DisplayValue)).ReturnsAsync(name);
            _service = new(null, mockParse.Object, null);

            var result = await _service.CreateScanElementAsync(barcode, ScanChoose.Штрихкод, "", false, true);

            Assert.Equal(name, result.Name);
        }

        [Fact]
        public async Task CreateScanElement_WithServerConnection_CorrectName()
        {
            var nameParse = "Новое название PARSE";
            var nameApi = "Новое название API";
            var guid = "0";
            var barcode = new BarcodeResult() { DisplayValue = "0", RawValue = "0", BarcodeFormat = BarcodeFormats.Ean13 };
            var scanElement = new ScanElement() { Name = nameApi };
            var mockParse = new Mock<ICodeParseService>();
            mockParse.Setup(m => m.FindNameByCode(barcode.DisplayValue)).ReturnsAsync(nameParse);
            var mockApi = new Mock<IScanApiService>();
            mockApi.Setup(m => m.GetScannedCodeInformationAsync(barcode, guid)).ReturnsAsync(scanElement);
            _service = new(mockApi.Object, mockParse.Object, null);

            var result = await _service.CreateScanElementAsync(barcode, ScanChoose.Штрихкод, guid, true, true);

            Assert.Equal(nameApi, result.Name);
        }

        [Theory]
        [InlineData((int)BarcodeFormats.Ean8, (int)ScanChoose.Штрихкод)]
        [InlineData((int)BarcodeFormats.Ean13, (int)ScanChoose.Штрихкод)]
        [InlineData((int)BarcodeFormats.Code39, (int)ScanChoose.Штрихкод)]
        [InlineData((int)BarcodeFormats.Code93, (int)ScanChoose.Штрихкод)]
        [InlineData((int)BarcodeFormats.Code128, (int)ScanChoose.Штрихкод)]
        [InlineData((int)BarcodeFormats.QRCode, (int)ScanChoose.Акциз)]
        [InlineData((int)BarcodeFormats.DataMatrix, (int)ScanChoose.Акциз)]
        public void IsCorrectChooseScannedCode_BarcodesAndScanChoose_Correct(int barcodeFormat, int scanChoose)
        {
            var barcode = new BarcodeResult() { BarcodeFormat = (BarcodeFormats)barcodeFormat };

            var result = _service.IsCorrectChooseScannedCode(barcode, (ScanChoose)scanChoose);

            Assert.True(result);
        }

    }
}