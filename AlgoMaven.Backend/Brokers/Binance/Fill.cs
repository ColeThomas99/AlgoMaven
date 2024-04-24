using System;
using System.Text.Json.Serialization;

namespace AlgoMaven.Backend.Brokers.Binance
{
	public class Fill
	{
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        [JsonPropertyName("qty")]
        public decimal Quantity { get; set; }
        [JsonPropertyName("commission")]
        public decimal Commission { get; set; }
        [JsonPropertyName("commissionAsset")]
        public string CommissionAsset { get; set; }
        [JsonPropertyName("tradeID")]
        public int TradeID { get; set; }
	}
}

