using System;
using System.Text.Json.Serialization;

namespace AlgoMaven.Core.MarketData.Binance.Responses
{
	public class BinanceInstrumentListResponse
	{
        [JsonPropertyName("timezone")]
        public string Timezone { get; set; }
        [JsonPropertyName("serverTime")]
        public long ServerTime { get; set; }
        [JsonPropertyName("rateLimits")]
        public List<BinanceRateLimit> RateLimits { get; set; }
        [JsonPropertyName("symbols")]
        public List<BinanceSymbolInfo> Symbols { get; set; }
    }
}

