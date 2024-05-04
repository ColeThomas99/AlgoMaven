using System;
namespace AlgoMaven.Backend.RiskControlMeasures
{
	public class StopLossRCM : RiskControlMeasureBase
	{
        public decimal TriggerPrice { get; set; }

        public StopLossRCM()
		{
		}

        public override bool HasTriggered(object args)
        {
            decimal currentPrice = (decimal)args;

            if (currentPrice <= TriggerPrice)
                return true;

            return false;
        }
    }
}

