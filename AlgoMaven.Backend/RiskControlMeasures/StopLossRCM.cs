using System;
namespace AlgoMaven.Backend.RiskControlMeasures
{
	public class StopLossRCM : RiskControlMeasureBase
	{
        public decimal TriggerPrice { get; set; }

        public StopLossRCM()
		{
            RCMAction = RCMAction.Sell | RCMAction.Terminate;
		}

        public override bool HasTriggered(object[] args)
        {
            decimal currentPrice = (decimal)args[0];

            if (currentPrice <= TriggerPrice)
                return true;

            return false;
        }
    }
}

