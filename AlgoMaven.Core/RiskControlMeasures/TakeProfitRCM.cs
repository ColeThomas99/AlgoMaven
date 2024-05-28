using System;
namespace AlgoMaven.Core.RiskControlMeasures
{
    public class TakeProfitRCM : RiskControlMeasureBase
    {
        public decimal TriggerPrice { get; set; }

        public TakeProfitRCM()
        {
            RCMAction = RCMAction.Buy | RCMAction.Terminate;
        }

        public override bool HasTriggered(object[] args)
        {
            decimal currentPrice = (decimal)args[0];

            if (currentPrice >= TriggerPrice)
                return true;

            return false;
        }
    }
}

