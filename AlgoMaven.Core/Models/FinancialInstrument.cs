using System;
using AlgoMaven.Core.Enums;

namespace AlgoMaven.Core.Models
{
	public class FinancialInstrument
	{
        public string Name { get; set; }
        public string TickerSYM { get; set; }
        public InstrumentType InstrumentType { get; set; }
    }
}

