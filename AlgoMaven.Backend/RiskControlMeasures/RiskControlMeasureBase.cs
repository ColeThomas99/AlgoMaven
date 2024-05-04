using System;
namespace AlgoMaven.Backend.RiskControlMeasures
{
	public abstract class RiskControlMeasureBase
	{
		public abstract bool HasTriggered(object args);
	}
}

