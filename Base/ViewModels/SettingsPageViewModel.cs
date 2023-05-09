using Base.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Base.ViewModels
{
    public class SettingsPageViewModel : INotifyPropertyChanged
    {
        private string _login = "";
        private string _password = "";
        private string _connectionString = "";
        private string _payDate = "";
        private string _guid = "";
        private string _organization = "";
        private string _warehouse = "";
        private string _typeOfPrices = "";
        private string _verifyText = "";

        public event PropertyChangedEventHandler PropertyChanged;

        public string Login
        {
            get => _login;
            set
            {
                if (_login != value)
                {
                    _login = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged();
                }
            }
        }
        public string ConnectionString
        {
            get => _connectionString;
            set
            {
                if (_connectionString != value)
                {
                    _connectionString = value;
                    OnPropertyChanged();
                }
            }
        }
        public string PayDate
        {
            get => _payDate;
            set
            {
                if (_payDate != value)
                {
                    _payDate = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Guid
        {
            get => _guid;
            set
            {
                if (_guid != value)
                {
                    _guid = value;
                    OnPropertyChanged();

                    if (_guid == "")
                        VerifyText = "Регистрация";
                    else
                        VerifyText = "Проверить настройки";
                }
            }
        }
        public string Organization
        {
            get => _organization;
            set
            {
                if (_organization != value)
                {
                    _organization = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Warehouse
        {
            get => _warehouse;
            set
            {
                if (_warehouse != value)
                {
                    _warehouse = value;
                    OnPropertyChanged();
                }
            }
        }
        public string TypeOfPrices
        {
            get => _typeOfPrices;
            set
            {
                if (_typeOfPrices != value)
                {
                    _typeOfPrices = value;
                    OnPropertyChanged();
                }
            }
        }
        public string VerifyText 
        { 
            get => _verifyText;
            set
            {
                if(_verifyText != value)
                {
                    _verifyText = value;
                    OnPropertyChanged();
                }
            }
        }

        public void SetSettings (UserSettings settings)
        {
            Login = settings.Login;
            Password = settings.Password;
            ConnectionString = settings.ConnectionString;
            PayDate = settings.PayDate;
            Guid = settings.Guid;
            Organization = settings.Organization;
            Warehouse = settings.Warehouse;
            TypeOfPrices = settings.TypeOfPrices;
        }

        public UserSettings GetUserSettings ()
        {
            return new UserSettings()
            {
                Login = Login,
                Password = Password,
                ConnectionString = ConnectionString,
                PayDate = PayDate,
                Guid = Guid,
                Organization = Organization,
                Warehouse = Warehouse,
                TypeOfPrices = TypeOfPrices
            };
        }

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
