using Base.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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

        public void AddScannedCode (ScanElement element)
        {
            var search = ScannedCodes.FirstOrDefault(e => e.ScannedCode == element.ScannedCode);
            if(search != null)
                search.CountChoise++;
            else
            ScannedCodes.Add(element);
        }

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
