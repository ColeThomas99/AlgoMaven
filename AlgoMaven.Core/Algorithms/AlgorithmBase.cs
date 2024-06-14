using System;
using AlgoMaven.Core.Enums;
using AlgoMaven.Core.Models;
using AlgoMaven.Core.RiskControlMeasures;

namespace AlgoMaven.Core.Algorithms
{
    public abstract class AlgorithmBase
    {
        public event EventHandler<TradeSignalArgs> OnBuySignalTriggered;
        public event EventHandler<TradeSignalArgs> OnSellSignalTriggered;
        public event EventHandler PriceUpdate;
        public ExchangeType LastTradeType { get; set; }
        public BotOptions Options { get; set; }
        public Dictionary<string, string> Auths { get; set; }
        public abstract Task Run(decimal currentPrice, long time, List<PriceUpdate> prices);
        protected FinancialInstrument Instrument;
        protected int BuyCount = 0;
        public bool IsRunning = false;
        protected AlgorithmBase Algorithm { get; set; }

        public AlgorithmBase(FinancialInstrument instrument)
        {
            Instrument = instrument;
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

        protected List<PriceUpdate> GetInstrumentPrices(out decimal price, out long time)
        {
            List<PriceUpdate> prices = new List<PriceUpdate>();
            time = 0;
            price = 0;

            lock (Globals.MarketAPIPrices)
            {
                prices = Globals.MarketAPIPrices
                   [Globals.MarketAPIRankings.First(x => x.Value.Item1 == InstrumentType.Crypto).Key
                   ].First(x => x.Item3 == Instrument.TickerSYM).Item1.ToList();
            }

            if (prices.Count > 0)
            {
                price = prices.First().Amount;
                time = prices.First().Time.ToUnixTimeMilliseconds();
            }

            Console.WriteLine($"CURRENT TIME {time} : {DateTimeOffset.FromUnixTimeMilliseconds(time)}");
            Console.WriteLine($"CURRENT PRICE {price}");

            return prices;
        }

        public async Task RunAlgorithm()
        {
            Console.WriteLine($"Algorithm Started: {Algorithm.GetType()}");
            IsRunning = true;
            BuyCount = 0;

            while (IsRunning)
            {
                decimal price = 0;
                long time = 0;
                List<PriceUpdate> prices = GetInstrumentPrices(out price, out time);

                if (!HasRCMTriggered(new object[] { price, time}))
                    await Algorithm.Run(price, time, prices);
#if DEBUG
                await Task.Delay(7000 * Options.TradingFrequency);
#else
                await Task.Delay(60000 * Options.TradingFrequency);
#endif
            }

            Console.WriteLine("Algorithm Stopped");
        }

        protected bool HasRCMTriggered(object[] args)
        {
            if (!ValidateRCMArgs(args))
                return false; //error logging todo

            decimal price = (decimal)args[0];
            long time = (long)args[1];
            bool result = false;

            foreach (RiskControlMeasureBase rcm in Options.RCMs)
            {
                if (rcm.HasTriggered(args))
                {
                    if ((rcm.RCMAction | RCMAction.Buy) != 0)
                        ExecuteTrade(price, time, ExchangeType.Buy);
                    if ((rcm.RCMAction | RCMAction.Sell) != 0)
                        ExecuteTrade(price, time, ExchangeType.Sell);
                    if ((rcm.RCMAction | RCMAction.Terminate) != 0)
                        IsRunning = false;

                    Console.WriteLine("RCM Triggered");
                    result = true;
                }
            }
            return result;
        }

        private bool ValidateRCMArgs(object[] args)
        {
            bool result = true;

            if (args.Length < 2)
                return false;
            if (decimal.TryParse(args[0].ToString(), out _) == false)
                return false;
            if (long.TryParse(args[1].ToString(), out _) == false)
                return false;

            return result;
        }

        protected void ExecuteTrade(decimal price, long time, ExchangeType type)
        {
            TradeSignalArgs args = new TradeSignalArgs();
            args.Instrument = Instrument;
            args.Amount = Options.BuyIncrement;
            args.Price = price;
            args.Time = time;
            if (type == ExchangeType.Buy)
                BuySignalEvent(args);
            else
            {
                args.SellAll = true;
                SellSignalEvent(args);
            }
        }
    }
}

