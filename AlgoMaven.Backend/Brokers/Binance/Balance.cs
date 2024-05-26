using System;
using System.Text.Json.Serialization;

namespace AlgoMaven.Backend.Brokers.Binance
{
	public class Balance
	{
        [JsonPropertyName("asset")]
        public string Asset { get; set; }
        [JsonPropertyName("free")]
        public decimal Free { get; set; }
        [JsonPropertyName("locked")]
        public decimal Locked { get; set; }
    }
}

