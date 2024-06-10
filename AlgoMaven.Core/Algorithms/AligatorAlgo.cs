using System;
using AlgoMaven.Core.Enums;
using AlgoMaven.Core.Models;

namespace AlgoMaven.Core.Algorithms
{
    /// <summary>
	/// This is an algorithm that follows the "Williams Aligator Indicator"
	/// </summary>
    public class AligatorAlgo : AlgorithmBase
    {
        public AligatorAlgo(FinancialInstrument instrument) : base(instrument)
        {
            Auths = new Dictionary<string, string>();
            Algorithm = this;
        }

        public override async Task Run(decimal currentPrice, long time, List<PriceUpdate> prices)
        {
            decimal jaw = CalculateSMA(13, prices);
            decimal teeth = CalculateSMA(8, prices);
            decimal lips = CalculateSMA(5, prices);

            if (lips > teeth)
            {
                if (lips >= jaw && teeth >= jaw)
                {
                    if (BuyCount + 1 <= Options.MaxBuyBeforeSell)
                    {
                        ExecuteTrade(currentPrice, time, ExchangeType.Buy);
                        LastTradeType = ExchangeType.Buy;
                        BuyCount++;
                    }
                }
            }
            else if (lips < teeth)
            {
                if (lips < jaw && teeth < jaw)
                {
                    if (LastTradeType != ExchangeType.Sell)
                    {
                        ExecuteTrade(currentPrice, time, ExchangeType.Sell);
                        LastTradeType = ExchangeType.Sell;
                        BuyCount = 0;
                    }
                }
            }
        }

        public decimal CalculateSMA(int period, List<PriceUpdate> prices)
        {
            //TODO if any price returns NULL then terminate the algorithm and terminate the bot
            List<decimal> result = new List<decimal>();

            for (int i = 0; i < period; i++)
                if (prices.Count >= i + 1)
                    result.Add(prices[i].Amount);

            if (result.Count > 0)
                return result.Average();
            else
                return 0;
        }
    }
}

