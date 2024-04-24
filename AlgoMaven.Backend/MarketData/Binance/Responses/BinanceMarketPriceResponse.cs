using System;
namespace AlgoMaven.Backend.MarketData.Binance.Responses
{
	public class BinanceMarketPriceResponse
    {
        public List<BinanceMarketCandle> Candles { get; set; }
    }
}

