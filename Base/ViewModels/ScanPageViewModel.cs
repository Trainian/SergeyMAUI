using Base.Enums;
using Base.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Base.ViewModels
{
    public class ScanPageViewModel : INotifyPropertyChanged
    {
        private ScanChoose _scanChoose = ScanChoose.Штрихкод;
        private ScanElement _scanElement;

        public event PropertyChangedEventHandler PropertyChanged;

        public ScanElement ScanElement 
        {
            get => _scanElement;
            set
            {
                if (_scanElement != value)
                {
                    _scanElement = value;
                    OnPropertyChanged();
                }
            }
        }


        public ScanChoose ScanChoose
        {
            get => _scanChoose;
            set
            {
                if (_scanChoose != value)
                {
                    _scanChoose = value;
                    OnPropertyChanged();
                }
            }
        }

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    }
}
