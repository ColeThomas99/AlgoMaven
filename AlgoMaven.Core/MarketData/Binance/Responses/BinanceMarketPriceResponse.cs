using System;
namespace AlgoMaven.Core.MarketData.Binance.Responses
{
	public class BinanceMarketPriceResponse
	{
        public List<BinanceMarketCandle> Candles { get; set; }
    }
}

