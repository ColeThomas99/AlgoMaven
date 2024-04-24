using System;
namespace AlgoMaven.Backend.Models
{
	public class BotOptions
	{
		public decimal BuyIncrement = 1;
		public decimal? BuyValueEqualToAmountUSD = null;
		public int MaxBuyBeforeSell = 3;
		public decimal SpendLimit = 1000;
		public decimal MaxSpendLimit = 2000;
	}
}

