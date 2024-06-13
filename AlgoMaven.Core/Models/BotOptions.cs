using System;
using AlgoMaven.Core.RiskControlMeasures;

namespace AlgoMaven.Core.Models
{
	public class BotOptions
	{
        public decimal BuyIncrement = 1;
        public decimal? BuyValueEqualToAmountUSD = null;
        public int MaxBuyBeforeSell = 3;
        public decimal SpendLimit = 1000;
        public decimal MaxSpendLimit = 2000;
        public List<RiskControlMeasureBase> RCMs = new List<RiskControlMeasureBase>();
        public bool EnableCaching = false;
        public int TradingFrequency = 5; //minutes
    }
}

