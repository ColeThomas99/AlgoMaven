using System;
using AlgoMaven.Core.Enums;
using AlgoMaven.Core.Models;

namespace AlgoMaven.Core.Algorithms
{
    public class BreakoutAlgo : AlgorithmBase
    {
        public BreakoutAlgo(FinancialInstrument instrument) : base(instrument)
        {
            Auths = new Dictionary<string, string>();
            Algorithm = this;
        }

        public override async Task Run(decimal currentPrice, long time, List<PriceUpdate> prices)
        {
            BreakOutArgs bArgs = HasBreakOut(prices, currentPrice);

            if (bArgs == BreakOutArgs.Buy)
            {
                if (BuyCount + 1 <= Options.MaxBuyBeforeSell)
                {
                    ExecuteTrade(currentPrice, time, ExchangeType.Buy);
                    LastTradeType = ExchangeType.Buy;
                    BuyCount++;
                }
            }
            else if (bArgs == BreakOutArgs.Sell)
            {
                if (LastTradeType != ExchangeType.Sell)
                {
                    ExecuteTrade(currentPrice, time, ExchangeType.Sell);
                    LastTradeType = ExchangeType.Sell;
                    BuyCount = 0;
                }
            }
        }

        public BreakOutArgs HasBreakOut(List<PriceUpdate> prices, decimal price,int period = 20)
        {

            decimal low = prices.Skip(1).Take(period).Min(x => x.Amount);
            decimal high = prices.Skip(1).Take(period).Max(x => x.Amount);

            if (price > high)
                return BreakOutArgs.Buy;
            else if (price < low)
                return BreakOutArgs.Sell;

            return BreakOutArgs.None;
        }
    }

    public enum BreakOutArgs
    {
        Buy,
        Sell,
        None
    }
}

