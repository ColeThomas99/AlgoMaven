using System;
namespace AlgoMaven.Backend.Models
{
	public class UserFinancialInstrument
	{
		public FinancialInstrument Instrument { get; set; }
		public decimal Quantity { get; set; }
		public decimal TotalValue { get; set; }
		/*
		 * convert TotalValue to function (returns decimal)
		 * use marketdataAPI to get the total value of the instrument * quantity at given time
		 * 
		 */
	}
}

