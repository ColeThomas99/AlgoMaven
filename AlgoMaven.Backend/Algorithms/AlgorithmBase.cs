using System;
using AlgoMaven.Backend.Enums;
using AlgoMaven.Backend.Models;

namespace AlgoMaven.Backend.Algorithms
{
	public abstract class AlgorithmBase
	{
        public event EventHandler<TradeSignalArgs> OnBuySignalTriggered;
        public event EventHandler<TradeSignalArgs> OnSellSignalTriggered;
        public event EventHandler PriceUpdate;
        public ExchangeType LastTradeType { get; set; }
        public BotOptions Options { get; set; }
        public Dictionary<string, string> Auths { get; set; }
        public abstract Task Run();
        protected FinancialInstrument Instrument;

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

        public void ExecuteTrade(decimal price, long time, ExchangeType type)
        {
            TradeSignalArgs args = new TradeSignalArgs();
            args.Instrument = Instrument;
            args.Amount = Options.BuyIncrement;
            args.Price = price;
            args.Time = time;
            args.SellAll = true;
            if (type == ExchangeType.Buy)
                BuySignalEvent(args);
            else
                SellSignalEvent(args);
        }
    }
}

