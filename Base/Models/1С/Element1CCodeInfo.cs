using Base.Models.Base;
using System.Text.Json.Serialization;

namespace Base.Models
{
    public class Element1CCodeInfo : Element1CBase<CardsList>
    {
        public ScanElement GetInformation(ScanElement scanElement)
        {
            scanElement.Name = this.Datas.Cards[0].Name;
            scanElement.QuantityLeft = this.Datas.Cards[0].QuantityLeft;
            scanElement.QuantityLeftNew = this.Datas.Cards[0].QuantityLeft;
            scanElement.Price = this.Datas.Cards[0].Price;
            scanElement.PriceNew = this.Datas.Cards[0].Price;
            return scanElement;
        }
    }
    public class CardsList 
    {
        [JsonPropertyName("nom_info")]
        public List<Card> Cards { get; set; } 
    }
    public class Card
    {
        [JsonPropertyName("Владелец")]
        public string Name { get; set; }
        [JsonPropertyName("КоличествоОстаток")]
        public int QuantityLeft { get; set; }
        [JsonPropertyName("Цена")]
        public decimal Price { get; set; }
    }
}
