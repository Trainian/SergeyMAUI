using BarcodeScanner.Mobile;
using Base.Enums;
using Base.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ZXing.QrCode.Internal;

namespace Base.ViewModels
{
    public class ScannedCodesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _selectedScannedCode;

        public ObservableCollection<ScanElement> ScannedCodes { get; set; } = new ObservableCollection<ScanElement>();

        public string SelectedScannedCode
        {
            get => _selectedScannedCode;
            set
            {
                if (_selectedScannedCode != value)
                {
                    _selectedScannedCode = value;
                    OnPropertyChanged();
                }
            }
        }

        public ScanElement LastScannedElement 
        {
            get => ScannedCodes.Last() ?? new ScanElement();
            set
            {
                if(ScannedCodes.Last() != value)
                {
                    var element = ScannedCodes.Last();
                    element = value;
                    OnPropertyChanged();
                }
            }
        }

        public Command<ScanElement> DeleteScanElement
        {
            get
            {
                return new Command<ScanElement>((scanElement) =>
                {
                    ScannedCodes.Remove(scanElement);
                });
            }
        }

        public Command DeleteScannedCodes
        {
            get
            {
                return new Command(() =>
                {
                    ScannedCodes.Clear();
                });
            }
        }

        public ScanElement AddOrUpdateScannedCode(ScanElement element, ScanChoose scanChoose)
        {
            var findElement = ScannedCodes.FirstOrDefault(e => e.Barcode == element.Barcode);

            if (findElement == null)
            {
                element.CountScanned++;
                ScannedCodes.Add(element);
                return element;
            }

            switch (scanChoose)
            {
                case ScanChoose.Штрихкод:
                    findElement.CountScanned++;
                    break;

                case ScanChoose.Акциз:
                    var qr = element.QRCodes.FirstOrDefault() ?? "";
                    var searchQR = findElement.QRCodes.FirstOrDefault(e => e == qr);

                    // Если данный QR отсутствует в списке добавляем
                    if (searchQR == null && qr != "")
                    {
                        findElement.QRCodes.Add(qr);
                    }

                    break;
            }
            return findElement;
        }

        public ScanElement SearchScannedCode(BarcodeResult barcode, ScanChoose scanChoose)
        {
            var scannedBarcode = barcode.DisplayValue;
            List<ScanElement> scannedCodesList;

            switch(scanChoose)
            {
                case ScanChoose.Штрихкод:
                    scannedCodesList = ScannedCodes.ToList();
                    foreach (var code in scannedCodesList)
                    {
                        if (code.Barcode == scannedBarcode)
                        {
                            return code;
                        }
                    }
                    break;
                case ScanChoose.Акциз:
                    scannedCodesList = ScannedCodes.Where(sc => sc.QRCodesCount > 0).ToList();
                    foreach (var code in scannedCodesList)
                    {
                        foreach (var qr in code.QRCodes)
                        {
                            if (qr == scannedBarcode)
                            {
                                return code;
                            }
                        }
                    }
                    break;
            }
            return null;
        }

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
