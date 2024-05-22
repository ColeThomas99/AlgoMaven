using System;
using AlgoMaven.Backend.Models;
using AlgoMaven.Backend.MarketData;
using AlgoMaven.Backend.Enums;
using AlgoMaven.Backend.RiskControlMeasures;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AlgoMaven.Backend.Algorithms
{
	/// <summary>
	/// This is an algorithm that follows the "Williams Aligator Indicator"
	/// </summary>
	public class AligatorAlgo : AlgorithmBase
	{
		public AligatorAlgo(FinancialInstrument instrument) : base(instrument)
		{
			Auths = new Dictionary<string, string>();
		}

		public override async Task Run()
		{
            Console.WriteLine("Algorithm Started");
			isRunning = true;
            int buyCount = 0;

            while (isRunning)
			{
				decimal price = 0;
				long time = 0;
				decimal jaw = CalculateSMA(13, out _, out _);
				decimal teeth = CalculateSMA(8, out _, out _);
				decimal lips = CalculateSMA(5, out price, out time);

				if (HasRCMTriggered(new object[] { price, time } ))
					goto SKIP;

				if (lips > teeth)
				{
					if (lips >= jaw && teeth >= jaw)
					{
						if (buyCount + 1 <= Options.MaxBuyBeforeSell)
						{
							ExecuteTrade(price, time, ExchangeType.Buy);
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
							ExecuteTrade(price, time, ExchangeType.Sell);
							LastTradeType = ExchangeType.Sell;
							buyCount = 0;
						}
                    }
				}

				SKIP:
#if DEBUG
				await Task.Delay(4000);
#else
				await Task.Delay(60000);
#endif
			}
			Console.WriteLine("Bot Stopped");
			//return null;
		}

		public decimal CalculateSMA(int period, out decimal price, out long time, bool debug = false, int index = 0)
		{
			//TODO if any price returns NULL then terminate the algorithm and terminate the bot
			//decimal value = 0;
			List<decimal> result = new List<decimal>();
			price = 0;
			time = 0;

			//List<PriceUpdate> prices = Globals.MarketAPIPrices["Binance"].First(x => x.Item3 == Instrument.TickerSYM).Item1.TakeLast(period).ToList();
			List<PriceUpdate> prices = Globals.MarketAPIPrices
				[ Globals.MarketAPIRankings.First(x => x.Value.Item1 == InstrumentType.Crypto).Key
				].First(x => x.Item3 == Instrument.TickerSYM).Item1.TakeLast(period).ToList();
            prices.Reverse();

			for (int i = 0; i < period; i++)
				if (prices.Count >= i + 1)
					result.Add(prices[i].Amount);

			if (prices.Count > 0)
			{
				price = prices[0].Amount;
				time = prices[0].Time.ToUnixTimeMilliseconds();
			}

			if (result.Count > 0)
				return result.Average();
			else
				return 0;
		}
	}
}

