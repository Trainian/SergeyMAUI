using Base.ViewModels;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Base.Models
{
    public class UserSettings
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string ConnectionString { get; set; }
        public string PayDate { get; set; }
        public string Guid { get; set; }
        public string Organization { get; set; }
        public string Warehouse { get; set; }
        public string TypeOfPrices { get; set; }
    }
}
