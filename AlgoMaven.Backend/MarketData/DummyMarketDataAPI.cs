using System;
using System.Globalization;
using AlgoMaven.Backend.Enums;
using AlgoMaven.Backend.Models;

namespace AlgoMaven.Backend.MarketData
{
	//Supports Only Crypto
	public class DummyMarketDataAPI : MarketDataAPIBase
	{
		Dictionary<FinancialInstrument, List<PriceUpdate>> Prices = new Dictionary<FinancialInstrument, List<PriceUpdate>>();
		public int Index = 0;

		public DummyMarketDataAPI()
		{
			APIEndPoint = "";
			Credentials = null;
            Name = "Dummy";
			InitPricesList();
		}

        public override Task InitMarketInstruments()
        {
            throw new NotImplementedException();
        }

        public override async Task<List<PriceUpdate>?> GetPricesAsync(FinancialInstrument instrument, long timeStampSeconds, long? endTimeStampSeconds, string? interval = null, int limit = 100)
        {
            if (Prices.Count == 0)
                InitPricesList();

			PriceUpdate? update = new PriceUpdate();

            List<PriceUpdate> updates = Prices.ElementAt(0).Value;//Prices[instrument];
            //update = updates.First(x => x.Time == DateTimeOffset.FromUnixTimeSeconds(timeStampSeconds));
            //update = updates[Index];

            if (updates.Count > Index && Index > 0)
                update = updates[Index];
            else
                update = null;

            return null;//todo
        }

		private void InitPricesList()
		{
			foreach (FinancialInstrument instrument in Globals.FinancialInstruments)
			{
                List<PriceUpdate> prices = new List<PriceUpdate>
                {
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 71949 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 71719 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 71773 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 71766 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 71720 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 71943 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 71729 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 71714 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 71710 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 71916 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 71636 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 71436 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 71277 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 71488 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 71294 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 71208 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 71058 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 70789 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 70407 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 70510 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 70396 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 70569 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 70787 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 70694 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 69953 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 69337 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 69254 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 68582 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 68796 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 68857 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 68980 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 69110 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 69187 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 69305 },/*
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 94 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 97 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 98 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 102 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 106 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 109 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 115 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 119 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 124 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 130 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 135 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 136 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 139 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 140 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 143 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 145 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 145 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 145 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 143 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 140 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 137 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 135 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 134 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 134 },
                    new PriceUpdate { Instrument = instrument, Time = DateTimeOffset.UtcNow, Amount = 133 },*/
                };

                Prices.Add(instrument, prices);
			}
		}
    }
}
