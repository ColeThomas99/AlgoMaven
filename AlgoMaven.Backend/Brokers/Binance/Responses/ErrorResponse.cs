using System;
using System.Text.Json.Serialization;

namespace AlgoMaven.Backend.Brokers.Binance.Responses
{
	public class ErrorResponse
	{
        [JsonPropertyName("code")]
        public int Code { get; set; }
        [JsonPropertyName("msg")]
        public string Message { get; set; }
	}
}

