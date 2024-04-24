using System;
namespace AlgoMaven.Backend.Models
{
	public class Wallet
	{
		public List<UserFinancialInstrument> UserInstruments { get; set; }
		public decimal AvailableFunds { get; set; }
		public decimal TotalValue { get; set; }
		public string Currency { get; set; }

		public Wallet()
		{
			UserInstruments = new List<UserFinancialInstrument>();
		}

		public void CalculateTotal()
		{
			foreach (UserFinancialInstrument instrument in UserInstruments)
				TotalValue += instrument.TotalValue;
		}
	}
}

