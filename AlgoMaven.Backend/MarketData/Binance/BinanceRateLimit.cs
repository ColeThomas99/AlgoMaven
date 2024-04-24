using System;
using System.Text.Json.Serialization;

namespace AlgoMaven.Backend.MarketData.Binance
{
	public class BinanceRateLimit
	{
        [JsonPropertyName("rateLimitType")]
        public string RateLimitType { get; set; }
        [JsonPropertyName("interval")]
        public string Interval { get; set; }
        [JsonPropertyName("intervalNum")]
        public int IntervalNumber { get; set; }
        [JsonPropertyName("limit")]
        public decimal Limit { get; set; }
    }
}

