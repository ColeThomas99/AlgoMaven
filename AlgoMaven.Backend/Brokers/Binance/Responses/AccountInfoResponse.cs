using System;
using System.Text.Json.Serialization;

namespace AlgoMaven.Backend.Brokers.Binance.Responses
{
	public class AccountInfoResponse
	{
        [JsonPropertyName("balances")]
        public List<Balance> Balances { get; set; }
        [JsonPropertyName("canTrade")]
        public bool CanTrade { get; set; }
	}
}
