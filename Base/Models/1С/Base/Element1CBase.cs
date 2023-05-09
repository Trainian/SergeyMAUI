using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Base.Models.Base
{
    public class Element1CBase<T>
    {
        [JsonPropertyName("Результат")]
        public bool Result { get; set; }
        [JsonPropertyName("Сообщение")]
        public string Message { get; set; }
        [JsonPropertyName("Данные")]
        public T Datas { get; set; }
    }
}
