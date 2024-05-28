using System;
using System.Text;
using AlgoMaven.Core.Algorithms;
using AlgoMaven.Core.Enums;
using AlgoMaven.Core.Models;

namespace AlgoMaven.Core.Brokers
{
	public abstract class BrokerBase
	{
        public Wallet Wallet { get; set; }
        public string URL { get; set; }
        public string Name { get; set; }
        public string APIKey { get; set; }
        public abstract Task Exchange(ExchangeType type, TradeSignalArgs args);
        public abstract Task<decimal> GetAvailableFunds();
        public abstract Task<bool> HasAvailableInstruments(FinancialInstrument instrument, decimal quantity);
        public abstract void AddInstrument(FinancialInstrument instrument, decimal quantity); //ONLY FOR LOCAL BROKERS (DUMMY)
        public abstract Task<List<FinancialInstrument>> GetAvailableInstruments();
        public abstract Task<decimal> GetQuantityOfIntrsument(FinancialInstrument instrument);
        public UserAccount User;
        public BrokerCredentials BrokerCredentials { get; set; }
        public Dictionary<string, string> PlatformArgs = new Dictionary<string, string>(); //api keys, secrets, signatures etc
        //public Dictionary<string, (InstrumentType ,int)> MarketAPIRankings = new Dictionary<string, (InstrumentType, int)>();
        //public List<FinancialInstrument> FinancialInstruments = new List<FinancialInstrument>();

        //ADD constructor which takes in user account as arg - use this instead of "wallet" variable

        public BrokerBase(ref UserAccount user)
        {
            User = user;
            Wallet = new Wallet();
        }

        public string GenerateURL(string endPoint, Dictionary<string, string> queries)
        {
            string result = endPoint.TrimEnd('/') + "&";

            foreach (KeyValuePair<string, string> query in queries)
                result += $"{query.Key}={query.Value}";

            return result;
        }

        public async Task<string> SendHTTPPost(string url, string request, Dictionary<string, string> headers)
        {
            string response = "";
            StringContent content = new StringContent(request, Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                foreach (KeyValuePair<string, string> header in headers)
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);

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
                foreach (KeyValuePair<string, string> header in headers)
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);

                response = await client.GetStringAsync(url);
            }

            return response;
        }
    }
}

