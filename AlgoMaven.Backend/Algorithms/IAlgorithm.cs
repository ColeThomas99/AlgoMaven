using System;
using AlgoMaven.Backend.Enums;
using AlgoMaven.Backend.Models;

namespace AlgoMaven.Backend.Algorithms
{
	public interface IAlgorithm
	{
		public event EventHandler<TradeSignalArgs> OnBuySignalTriggered;
		public event EventHandler<TradeSignalArgs> OnSellSignalTriggered;
		public event EventHandler PriceUpdate;
		public ExchangeType LastTradeType { get; set; }
        public BotOptions Options { get; set; }
        public Dictionary<string, string> Auths { get; set; }
        public Task Run();
	}

	public class TradeSignalArgs
	{
		public FinancialInstrument Instrument { get; set; }
		public decimal Amount { get; set; }
		public decimal Price { get; set; }
		public bool SellAll { get; set; }
		public long Time { get; set; }
		public Dictionary<string, string> Auths = new Dictionary<string, string>();
	}
}

