using System;
using AlgoMaven.Backend.Algorithms;
using AlgoMaven.Backend.Models;
using AlgoMaven.Backend.Brokers;

namespace AlgoMaven.Backend.Bots
{
	public class StandardBot : BotBase
	{
		public StandardBot(/*IAlgorithm algorithm, UserAccount user, FinancialInstrument instrument, BrokerBase broker*/) //: base(/*algorithm, user, instrument, broker*/)
		{
			Options.MaxSpendLimit = 2000;
		}

        public override async Task Run()
        {
			await base.Run();
			Console.WriteLine("Bot Started");
			await Algorithm.Run();
        }
    }
}

