using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Base.Models
{
    public class ScanElement : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _scannedCode = "0000000000000";
        private string _name = "Номенклатура не найдена";
        private int _quantityLeft = 0;
        private decimal _price = 0.0M;
        private int _countChoise = 1;

        public string ScannedCode
        {
            get => _scannedCode;
            set
            {
                if (_scannedCode != value)
                {
                    _scannedCode = value;
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

        public decimal Price 
        { 
            get => _price;
            set
            {
                if(_price != value)
                {
                    _price = value;
                    OnPropertyChanged();
                }
            }
        }

        public int CountChoise 
        { 
            get => _countChoise;
            set
            {
                if (_countChoise != value)
                {
                    _countChoise = value;
                    OnPropertyChanged();
                }
            }
        }

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
