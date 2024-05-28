using System;
using AlgoMaven.Core.Brokers;

namespace AlgoMaven.Core.Models
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

