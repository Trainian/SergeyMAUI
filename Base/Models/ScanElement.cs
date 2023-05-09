using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Base.Models
{
    public class ScanElement : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _barcode = "0000000000000";
        private string _name = "Новая номенклатура";
        private int _quantityLeft = -1;
        private int _quantityLeftNew = -1;
        private decimal _price = -0.1M;
        private decimal _priceNew = -0.1M;
        private int _countScanned = 0;
        private string _details = "";
        private bool _isVisibleQRCodes = false;
        private ObservableCollection<string> _qrCodes = new ObservableCollection<string>();

        public string Barcode
        {
            get => _barcode;
            set
            {
                if (_barcode != value)
                {
                    _barcode = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        public int QuantityLeft
        {
            get => _quantityLeft;
            set
            {
                if (_quantityLeft != value)
                {
                    _quantityLeft = value;
                    OnPropertyChanged();
                }
            }
        }

        public int QuantityLeftNew
        {
            get => _quantityLeftNew;
            set
            {
                if (_quantityLeftNew != value)
                {
                    _quantityLeftNew = value;
                    OnPropertyChanged();
                    ChangeDetails();
                }
            }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                if (_price != value)
                {
                    _price = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal PriceNew
        {
            get => _priceNew;
            set
            {
                if (_priceNew != value)
                {
                    _priceNew = value;
                    OnPropertyChanged();
                    ChangeDetails();
                }
            }
        }

        public int CountScanned
        {
            get => _countScanned;
            set
            {
                if (_countScanned != value)
                {
                    _countScanned = value;
                    OnPropertyChanged();
                    ChangeDetails();
                }
            }
        }

        public string Details 
        {
            get => _details;
            set
            {
                if(_details != value)
                {
                    _details = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<string> QRCodes 
        { 
            get => _qrCodes; 
            set  
            {
                var except = _qrCodes.Except(value);
                if(except.Count() != 0)
                {
                    _qrCodes = value;
                    OnPropertyChanged();
                };
            }
        }

        public int QRCodesCount { get => _qrCodes.Count; }

        public bool IsVisibleQRCodes 
        { 
            get => _isVisibleQRCodes;
            set
            {
                if(_isVisibleQRCodes != value)
                {
                    _isVisibleQRCodes = value;
                    OnPropertyChanged();
                }
            }
        }

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private void ChangeDetails()
        {
            Details = "остаток: " + QuantityLeftNew + "шт., цена: " + PriceNew + "руб.\nОтсканировано: " + CountScanned + "шт.";
        }
    }
}
