using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Base.ViewModels
{
    public class ScanPageViewModel : INotifyPropertyChanged
    {
        private string _scanText = "";

        public event PropertyChangedEventHandler PropertyChanged;

        public string ScanText 
        { 
            get => _scanText; 
            set
            {
                if(_scanText != value)
                {
                    _scanText = value;
                    OnPropertyChanged();
                }
            } 
        }

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
