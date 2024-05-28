using System;
using System.Text.Json.Serialization;

namespace AlgoMaven.Core.MarketData.Binance
{
	public class BinanceSymbolInfo
	{
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }
        [JsonPropertyName("baseAsset")]
        public string BaseAsset { get; set; }
        [JsonPropertyName("quoteAsset")]
        public string QuoteAsset { get; set; }
    }
}

