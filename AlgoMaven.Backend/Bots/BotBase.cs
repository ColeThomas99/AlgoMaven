using System;
using AlgoMaven.Backend.Brokers;
using AlgoMaven.Backend.Enums;
using AlgoMaven.Backend.Algorithms;
using AlgoMaven.Backend.Models;

namespace AlgoMaven.Backend.Bots
{
	public abstract class BotBase
	{
		public BrokerBase Broker { get; set; }
		public AlgorithmBase Algorithm { get; set; }
        public UserAccount User { get; set; }
        public FinancialInstrument Instrument { get; set; }
        public DateTimeOffset Start { get; set; }
        public BotOptions Options = new BotOptions();

		public BotBase(/*IAlgorithm algorithm, UserAccount user, FinancialInstrument instrument, BrokerBase broker*/)
		{
            /*Algorithm = algorithm;
            User = user;
            Instrument = instrument;
            Broker = broker;*/
            //init event handlers for algorithms
            //when event is raised
            //	call buy OR sell with required args
            //Algorithm.OnBuySignalTriggered += Algorithm_OnBuySignalTriggered;
            //Algorithm.OnSellSignalTriggered += Algorithm_OnSellSignalTriggered;
		}

        public async Task Init()
        {
            Algorithm.OnBuySignalTriggered += Algorithm_OnBuySignalTriggered;
            Algorithm.OnSellSignalTriggered += Algorithm_OnSellSignalTriggered;
        }

        private void Algorithm_OnSellSignalTriggered(object? sender, TradeSignalArgs args)
        {
            Broker.Exchange(ExchangeType.Sell, args);
        }

        private void Algorithm_OnBuySignalTriggered(object? sender, TradeSignalArgs args)
        {
            Broker.Exchange(ExchangeType.Buy, args);
        }

		public virtual async Task Run()
        {
            if (Algorithm == null)
                throw new Exception("Algorithm Cannot be Null");
            if (User == null)
                throw new Exception("User Cannot be Null");
            if (Broker == null)
                throw new Exception("Broker Cannot be Null");
            /*if (Instrument == null)
                throw new Exception("Instrument Cannot be Null");*/

            await Init();

            Start = DateTimeOffset.UtcNow;
        }
	}
}

