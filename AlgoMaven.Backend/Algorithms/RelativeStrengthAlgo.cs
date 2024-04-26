using System;
using System.Diagnostics.Metrics;
using AlgoMaven.Backend.Enums;
using AlgoMaven.Backend.Models;

namespace AlgoMaven.Backend.Algorithms
{
    public class RelativeStrengthAlgo : IAlgorithm
    {
        public event EventHandler<TradeSignalArgs> OnBuySignalTriggered;
        public event EventHandler<TradeSignalArgs> OnSellSignalTriggered;
        public event EventHandler PriceUpdate;
        private FinancialInstrument Instrument;
        public Dictionary<DateTimeOffset, decimal> Prices = new Dictionary<DateTimeOffset, decimal>();
        public ExchangeType LastTradeType { get; set; }
        public BotOptions Options { get; set; }
        public Dictionary<string, string> Auths { get; set; }

        public RelativeStrengthAlgo(FinancialInstrument instrument)
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
                decimal rsi = CalculateRSI(out price, out time);
                if (rsi <= 30)
                {
                    //buy
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
                else if (rsi >= 70)
                {
                    //sell
                    if (LastTradeType != ExchangeType.Sell)
                    {
                        TradeSignalArgs args = new TradeSignalArgs();
                        args.Instrument = Instrument;
                        args.Amount = Options.BuyIncrement;
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

        public decimal CalculateRSI(out decimal price, out long time, int period = 14)
        {
            decimal result = 0;
            price = 0;
            time = 0;

            List<PriceUpdate> prices = Globals.MarketAPIPrices
               [Globals.MarketAPIRankings.First(x => x.Value.Item1 == InstrumentType.Crypto).Key
               ].First(x => x.Item3 == Instrument.TickerSYM).Item1.TakeLast(period).ToList();
            prices.Reverse();

            List<decimal> profits = new List<decimal>();
            List<decimal> losses = new List<decimal>();

            for (int i = 1; i < prices.Count; i++)
            {
                decimal change = prices[i].Amount - prices[i - 1].Amount;
                if (change >= 0)
                    profits.Add(change);
                else
                {
                    losses.Add(Math.Abs(change));
                    //Console.WriteLine(change);
                    //Console.WriteLine(Math.Abs(change));
                }
            }

            if (prices.Count > 0)
            {
                price = prices[0].Amount;
                time = prices[0].Time.ToUnixTimeMilliseconds();
            }

            decimal avgProfits = 1;
            decimal avgLosses = 1;

            if (profits.Count > 0)
                avgProfits = profits.Average();
            if (losses.Count > 0)
                avgLosses = losses.Average();

            decimal RSIone = 100 - ( 100 / (1 + (avgProfits / avgLosses)));
            //RSI second step todo

            return RSIone;
            //return result;
        }
    }
}

