using Base.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Base.Models
{
    public class Element1CSettings : Element1CBase<Data>
    {
        public UserSettings GetInformation(UserSettings settings)
        {
            if (String.IsNullOrWhiteSpace(this.Datas.Login) != true) settings.Login = this.Datas.Login;
            if (String.IsNullOrWhiteSpace(this.Datas.PayDate) != true) settings.PayDate = this.Datas.PayDate;
            if (String.IsNullOrWhiteSpace(this.Datas.Guid) != true) settings.Guid = this.Datas.Guid;
            return settings;
        }
    }
    public class Data
    {
        [JsonPropertyName("Логин")]
        public string Login { get; set; }
        [JsonPropertyName("ТарифДействуетПо")]
        public string PayDate { get; set; }
        [JsonPropertyName("НовыйУИД")]
        public string Guid { get; set; }
    }
}
