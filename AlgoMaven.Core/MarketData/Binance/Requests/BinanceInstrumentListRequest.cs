using System;
namespace AlgoMaven.Core.MarketData.Binance.Requests
{
	public class BinanceInstrumentListRequest
	{
        public string? Symbol { get; set; }
        public string[]? Symbols { get; set; }
        public string[]? Permissions { get; set; }
    }
}

