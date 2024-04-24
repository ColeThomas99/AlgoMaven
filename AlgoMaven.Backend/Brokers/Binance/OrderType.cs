using System;
namespace AlgoMaven.Backend.Brokers.Binance
{
	public enum OrderType
	{
		LIMIT,
		MARKET,
		STOP_LOSS,
		STOP_LOSS_LIMIT,
		TAKE_PROFIT,
		TAKE_PROFIT_LIMIT,
		LIMIT_MAKER
	}
}

