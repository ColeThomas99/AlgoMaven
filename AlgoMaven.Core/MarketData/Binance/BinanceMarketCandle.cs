using System;
namespace AlgoMaven.Core.MarketData.Binance
{
	public class BinanceMarketCandle
	{
        public long OpenTime { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal Volume { get; set; }
        public long CloseTime { get; set; }
        public decimal BaseAssetVolume { get; set; }
        public int TradeCount { get; set; }
        public decimal TakerBuyVolume { get; set; }
        public decimal TakeBuyBaseAssetVolume { get; set; }
        public decimal Ignore { get; set; }
    }
}

