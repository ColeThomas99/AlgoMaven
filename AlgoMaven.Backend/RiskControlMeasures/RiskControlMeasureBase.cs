using System;
namespace AlgoMaven.Backend.RiskControlMeasures
{
	public abstract class RiskControlMeasureBase
	{
		public abstract bool HasTriggered(object[] args);
		public RCMAction RCMAction { get; set; }
	}

	public enum RCMAction
	{
		Buy = 1, // 0001
		Sell = 2, //0010
		None = 4, //0100
		Terminate = 8 //1000
	}
}

