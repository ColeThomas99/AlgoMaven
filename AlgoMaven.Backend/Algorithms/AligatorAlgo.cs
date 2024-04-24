using System;
using AlgoMaven.Backend.Models;
using AlgoMaven.Backend.MarketData;
using AlgoMaven.Backend.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AlgoMaven.Backend.Algorithms
{
	public class AligatorAlgo : IAlgorithm
	{
        public event EventHandler<TradeSignalArgs> OnBuySignalTriggered;
        public event EventHandler<TradeSignalArgs> OnSellSignalTriggered;
		public event EventHandler PriceUpdate;
		private FinancialInstrument Instrument;
		public Dictionary<DateTimeOffset, decimal> Prices = new Dictionary<DateTimeOffset, decimal>();
		public ExchangeType LastTradeType { get; set; }
		public BotOptions Options { get; set; }
        public Dictionary<string, string> Auths { get; set; }

        public AligatorAlgo(FinancialInstrument instrument /* user args here */)
		{
			Instrument = instrument;
			Auths = new Dictionary<string, string>();
			/*
			 * configure price update event
			 *	whenever theres a price update
			 *		calculate using algo
			 *			determine action (sell,buy,nothing)
			 */
		}

		protected virtual void BuySignalEvent(TradeSignalArgs args)
		{
            EventHandler<TradeSignalArgs> handler = OnBuySignalTriggered;
            if (handler != null)
                handler(this, args);
        }

		protected virtual void SellSignalEvent(TradeSignalArgs args)
		{
            EventHandler<TradeSignalArgs> handler = OnSellSignalTriggered;
            if (handler != null)
                handler(this, args);
        }

		public async Task Run()
		{
            Console.WriteLine("Algorithm Started");
            int buyCount = 0;

            while (true)
			{
				decimal price = 0;
				long time = 0;
				decimal jaw = CalculateSMA(13, out _, out _/*,true, i*/);
				decimal teeth = CalculateSMA(8, out _, out _/*, true, i*/);
				decimal lips = CalculateSMA(5, out price, out time/*, true, i*/);
				//int buyCount = 0;

				if (lips > teeth)
				{
					if (lips >= jaw && teeth >= jaw)
					{
						if (buyCount + 1 <= Options.MaxBuyBeforeSell)
						{
							TradeSignalArgs args = new TradeSignalArgs();
							args.Instrument = Instrument;
							args.Amount = Options.BuyIncrement;
							args.Price = price;
							args.Time = time;
							BuySignalEvent(args);
							LastTradeType = ExchangeType.Buy;
							buyCount++;
						}
                    }
				}
				else if (lips < teeth)
				{
					if (lips < jaw && teeth < jaw)
					{
						if (LastTradeType != ExchangeType.Sell)
						{
							TradeSignalArgs args = new TradeSignalArgs();
							args.Instrument = Instrument;
							args.Amount = Options.BuyIncrement; //this will lead to an error on a live market (check how many is available in wallet first)
							args.Price = price;
							args.SellAll = true;
							args.Time = time;
							SellSignalEvent(args);
							LastTradeType = ExchangeType.Sell;
							buyCount = 0;
						}
                    }
				}

				await Task.Delay(4000);
			}

			//return null;
		}

		public decimal CalculateSMA(int period, out decimal price, out long time, bool debug = false, int index = 0)
		{
			//TODO if any price returns NULL then terminate the algorithm and terminate the bot
			decimal value = 0;
			price = 0;
			time = 0;

			//List<PriceUpdate> prices = Globals.MarketAPIPrices["Binance"].First(x => x.Item3 == Instrument.TickerSYM).Item1.TakeLast(period).ToList();
			List<PriceUpdate> prices = Globals.MarketAPIPrices
				[ Globals.MarketAPIRankings.First(x => x.Value.Item1 == InstrumentType.Crypto).Key
				].First(x => x.Item3 == Instrument.TickerSYM).Item1.TakeLast(period).ToList();
            prices.Reverse();

			for (int i = 0; i < period; i++)
				if (prices.Count >= i + 1)
					value += prices[i].Amount;

			if (prices.Count > 0)
			{
				price = prices[0].Amount;
				time = prices[0].Time.ToUnixTimeMilliseconds();
			}

            return value / period;
		}
	}
}

