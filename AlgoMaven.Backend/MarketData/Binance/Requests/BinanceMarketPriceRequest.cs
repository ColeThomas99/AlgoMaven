using System;
using AlgoMaven.Backend.MarketData.Binance;

namespace AlgoMaven.Backend.MarketData.Binance.Requests
{
	public class BinanceMarketPriceRequest
	{
        public string Symbol { get; set; }
        public BinanceMarketIntervals Interval { get; set; }
        public long? StartTime { get; set; }
        public long? EndTime { get; set; }
        public int? Limit { get; set; }
    }
}

