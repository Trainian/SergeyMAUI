using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Base.ViewModels
{
    public class SpeechPageViewModel : INotifyPropertyChanged
    {
        private string _speechText = "";

        public event PropertyChangedEventHandler PropertyChanged;

        public string SpeechText { 
            get => _speechText;
            set 
            {
                if (_speechText != value)
                {
                    _speechText = value;
                    OnPropertyChanged();
                }
            }
        }

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
