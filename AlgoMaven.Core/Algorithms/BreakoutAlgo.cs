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
        }

        public override async Task Run()
        {
            isRunning = true;
            int buyCount = 0;
            while (isRunning)
            {
                decimal price = 0;
                long time = 0;
                BreakOutArgs bArgs = HasBreakOut(out price, out time);

                if (HasRCMTriggered(new object[] { price, time }))
                    goto SKIP;

                if (bArgs == BreakOutArgs.Buy)
                {
                    if (buyCount + 1 <= Options.MaxBuyBeforeSell)
                    {
                        ExecuteTrade(price, time, ExchangeType.Buy);
                        LastTradeType = ExchangeType.Buy;
                        buyCount++;
                    }
                }
                else if (bArgs == BreakOutArgs.Sell)
                {
                    if (LastTradeType != ExchangeType.Sell)
                    {
                        ExecuteTrade(price, time, ExchangeType.Sell);
                        LastTradeType = ExchangeType.Sell;
                        buyCount = 0;
                    }
                }

            SKIP:
#if DEBUG
                await Task.Delay(4000);
#else
                await Task.Delay(60000);
#endif
            }
        }

        public BreakOutArgs HasBreakOut(out decimal price, out long time, int period = 20)
        {
            price = 0;
            time = 0;

            List<PriceUpdate> prices = Globals.MarketAPIPrices
                [Globals.MarketAPIRankings.First(x => x.Value.Item1 == InstrumentType.Crypto).Key
                ].First(x => x.Item3 == Instrument.TickerSYM).Item1.TakeLast(period).ToList();
            prices.Reverse();

            if (prices.Count > 0)
            {
                price = prices[0].Amount;
                time = prices[0].Time.ToUnixTimeMilliseconds();
            }
            else
                return BreakOutArgs.None;

            decimal low = prices.Skip(1).Take(period).Min(x => x.Amount);
            decimal high = prices.Skip(1).Take(period).Max(x => x.Amount);

            if (price > high)
                return BreakOutArgs.Buy; //trigger buy signal
            else if (price < low)
                return BreakOutArgs.Sell; //trigger sell signal

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

