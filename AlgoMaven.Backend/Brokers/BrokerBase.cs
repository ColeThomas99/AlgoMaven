using System;
using AlgoMaven.Backend.Models;
using AlgoMaven.Backend.Enums;
using AlgoMaven.Backend.Algorithms;
using System.Text;

namespace AlgoMaven.Backend.Brokers
{
	public abstract class BrokerBase
	{
        public Wallet Wallet { get; set; }
        public string URL { get; set; }
        public string Name { get; set; }
        public string APIKey { get; set; }
        public abstract Task Exchange(ExchangeType type, TradeSignalArgs args);
        public abstract decimal GetAvailableFunds();
        public abstract bool HasAvailableInstruments(FinancialInstrument instrument, decimal quantity);
        public abstract void AddInstrument(FinancialInstrument instrument, decimal quantity); //ONLY FOR LOCAL BROKERS (DUMMY)
        public abstract List<FinancialInstrument> GetAvailableInstruments();
        public abstract decimal GetQuantityOfIntrsument(FinancialInstrument instrument);
        public UserAccount User;
        public Dictionary<string, string> PlatofrmArgs = new Dictionary<string, string>(); //api keys, secrets, signatures etc
        //public Dictionary<string, (InstrumentType ,int)> MarketAPIRankings = new Dictionary<string, (InstrumentType, int)>();
        //public List<FinancialInstrument> FinancialInstruments = new List<FinancialInstrument>();

        //ADD constructor which takes in user account as arg - use this instead of "wallet" variable

        public BrokerBase(ref UserAccount user)
        {
            User = user;
            Wallet = new Wallet();
        }

        public async Task<string> SendHTTPPost(string url, string request, Dictionary<string, string> headers)
        {
            string response = "";
            StringContent content = new StringContent(request, Encoding.UTF8, "application/json");
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage responseMessage = await client.PostAsync(url, content);
                response = responseMessage.Content.ReadAsStringAsync().Result;
            }
            return response;
        }

        public async Task<string> SendHTTPGet(string url, Dictionary<string, string> headers)
        {
            string response = "";
            using (HttpClient client = new HttpClient())
            {
                response = await client.GetStringAsync(url);
            }
            return response;
        }
    }
}

