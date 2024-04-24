using System;
using AlgoMaven.Backend.Brokers;

namespace AlgoMaven.Backend.Models
{
	public class UserAccount
	{
		public List<BrokerBase> BrokerPlatforms { get; set; }
		public int AccessLevel { get; set; }
		public string MembershipLevel { get; set; }
		public string ID { get; set; }

		public UserAccount()
		{
			BrokerPlatforms = new List<BrokerBase>();
		}
	}
}

