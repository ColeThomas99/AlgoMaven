using System;
using AlgoMaven.Core.Enums;
using AlgoMaven.Core.Models;

namespace AlgoMaven.Core.Algorithms
{
    public class RelativeStrengthAlgo : AlgorithmBase
    {
        public RelativeStrengthAlgo(FinancialInstrument instrument) : base(instrument)
        {
            Auths = new Dictionary<string, string>();
            Algorithm = this;
        }

        public override async Task Run(decimal currentPrice, long time, List<PriceUpdate> prices)
        {
            decimal rsi = CalculateRSI(prices);

            if (rsi <= 30)
            {
                if (BuyCount + 1 <= Options.MaxBuyBeforeSell)
                {
                    ExecuteTrade(currentPrice, time, ExchangeType.Buy);
                    LastTradeType = ExchangeType.Buy;
                    BuyCount++;
                }
            }
            else if (rsi >= 70)
            {
                if (LastTradeType != ExchangeType.Sell)
                {
                    ExecuteTrade(currentPrice, time, ExchangeType.Sell);
                    LastTradeType = ExchangeType.Sell;
                    BuyCount = 0;
                }
            }
        }

        public decimal CalculateRSI(List<PriceUpdate> prices, int period = 14)
        {
            decimal result = 0;

            List<decimal> profits = new List<decimal>();
            List<decimal> losses = new List<decimal>();

            for (int i = 1; i < prices.Count; i++)
            {
                decimal change = prices[i].Amount - prices[i - 1].Amount;
                if (change >= 0)
                    profits.Add(change);
                else
                    losses.Add(Math.Abs(change));
            }

            decimal avgProfits = 1;
            decimal avgLosses = 1;

            if (profits.Count > 0)
                avgProfits = profits.Average();
            if (losses.Count > 0)
                avgLosses = losses.Average();

            result = 100 - (100 / (1 + (avgProfits / avgLosses)));

            return result;
        }
    }
}

