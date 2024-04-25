using System;
using System.Diagnostics.Metrics;
using AlgoMaven.Backend.Enums;
using AlgoMaven.Backend.Models;

namespace AlgoMaven.Backend.Algorithms
{
	public class BreakoutAlgo : IAlgorithm
	{
        public event EventHandler<TradeSignalArgs> OnBuySignalTriggered;
        public event EventHandler<TradeSignalArgs> OnSellSignalTriggered;
        public event EventHandler PriceUpdate;
        private FinancialInstrument Instrument;
        public Dictionary<DateTimeOffset, decimal> Prices = new Dictionary<DateTimeOffset, decimal>();
        public ExchangeType LastTradeType { get; set; }
        public BotOptions Options { get; set; }
        public Dictionary<string, string> Auths { get; set; }

        public BreakoutAlgo(FinancialInstrument instrument /* user args here */)
        {
            Instrument = instrument;
            Auths = new Dictionary<string, string>();
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
            int buyCount = 0;
            while (true)
            {
                decimal price = 0;
                long time = 0;
                BreakOutArgs bArgs = HasBreakOut(out price, out time);
                if (bArgs == BreakOutArgs.Long)
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
                else if (bArgs == BreakOutArgs.Short)
                {
                    if (LastTradeType != ExchangeType.Sell)
                    {
                        //sell
                        TradeSignalArgs args = new TradeSignalArgs();
                        args.Instrument = Instrument;
                        args.Amount = Options.BuyIncrement; //this will lead to an error on a live market (check how many is available in wallet first) todo
                        args.Price = price;
                        args.SellAll = true;
                        args.Time = time;
                        SellSignalEvent(args);
                        LastTradeType = ExchangeType.Sell;
                        buyCount = 0;
                    }
                }
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

            decimal low = prices.Skip(1).Take(period).Min(x => x.Amount);
            decimal high = prices.Skip(1).Take(period).Max(x => x.Amount);

            if (prices.Count > 0)
            {
                price = prices[0].Amount;
                time = prices[0].Time.ToUnixTimeMilliseconds();
            }

            if (price > high)
                return BreakOutArgs.Long; //trigger buy signal
            else if (price < low)
                return BreakOutArgs.Short; //trigger sell signal

            return BreakOutArgs.None;
        }
    }

    public enum BreakOutArgs
    {
        Long,
        Short,
        None
    }
}

