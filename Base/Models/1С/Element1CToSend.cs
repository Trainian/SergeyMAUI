using System.Text.Json.Serialization;

namespace Base.Models
{
    public class Element1CToSend
    {
        [JsonPropertyName("list_info")]
        public List<Card> cards { get; set; } = new List<Card>();

        public class Card
        {
            [JsonPropertyName("Штрихкод")]
            public string Barcode { get; set; }
            [JsonPropertyName("Количество")]
            public int CountScanned { get; set; }
            [JsonPropertyName("Цена")]
            public decimal Price { get; set; }
        }

        public void AddCardToList(ScanElement element)
        {
            Card card = new Card();
            card.Barcode = element.Barcode;
            card.CountScanned = element.CountScanned;
            card.Price = element.PriceNew;
        }
    }
}
