using System;
namespace AlgoMaven.Backend.Models
{
	public class PriceUpdate
	{
		public FinancialInstrument Instrument { get; set; }
		public DateTimeOffset Time { get; set; }
		public decimal Amount { get; set; }
	}
}

