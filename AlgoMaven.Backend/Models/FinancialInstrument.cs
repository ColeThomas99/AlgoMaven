using System;
using AlgoMaven.Backend.Enums;

namespace AlgoMaven.Backend.Models
{
	public class FinancialInstrument
	{
		public string Name { get; set; }
		public string TickerSYM { get; set; }
		public InstrumentType InstrumentType { get; set; }
	}
}

