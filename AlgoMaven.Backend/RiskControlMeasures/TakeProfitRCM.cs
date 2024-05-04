using System;
namespace AlgoMaven.Backend.RiskControlMeasures
{
	public class TakeProfitRCM : RiskControlMeasureBase
	{
        public decimal TriggerPrice { get; set; }

		public TakeProfitRCM()
		{

		}
         
        public override bool HasTriggered(object args)
        {
            decimal currentPrice = (decimal)args;

            if (currentPrice >= TriggerPrice)
                return true;

            return false;
        }
    }
}

