using System;
using AlgoMaven.Core.Models;

namespace AlgoMaven.Core.MarketData
{
	public abstract class MarketDataAPIBase
	{
        public string APIEndPoint { get; set; }
        public string Credentials { get; set; }
        public string Name { get; set; }
        public abstract Task InitMarketInstruments();
        public abstract Task<List<PriceUpdate>?> GetPricesAsync(FinancialInstrument instrument, long startTimeStampSeconds, long? endTimeStampSeconds, string? interval = null, int limit = 100); //need to do range, need to store result
    }
}

