using System;
namespace AlgoMaven.Core.Models
{
	public class PriceUpdate
	{
        public FinancialInstrument Instrument { get; set; }
        public DateTimeOffset Time { get; set; }
        public decimal Amount { get; set; }
    }
}

