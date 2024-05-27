using System;
using AlgoMaven.Backend.Algorithms;
using AlgoMaven.Backend.Brokers.Binance.Requests;
using AlgoMaven.Backend.Enums;
using AlgoMaven.Backend.Models;
using AlgoMaven.Backend.Brokers.Binance;
using AlgoMaven.Backend.Brokers.Binance.Responses;
using AlgoMaven.Backend.Analytics;
using System.Text.Json;
using System.Text;
using System.Security.Cryptography;

namespace AlgoMaven.Backend.Brokers
{
	public class BinanceBroker : BrokerBase
	{
		public BinanceBroker(ref UserAccount user ) : base(ref user)
		{
            Name = "Binance";
            URL = "https://www.binance.com";
            PlatformArgs.Add("ORDERSAPI", Path.Combine(URL, "/api/v3/order"));
            PlatformArgs.Add("ACCOUNTINFO", Path.Combine(URL, "/api/v3/account"));
		}

        public override void AddInstrument(FinancialInstrument instrument, decimal quantity)
        {
            throw new NotImplementedException();
        }

        public override async Task Exchange(ExchangeType type, TradeSignalArgs args)
        {
            FinancialInstrument instrument = args.Instrument;
            decimal quantity = 0;

            if (args.SellAll)
                quantity = await GetQuantityOfIntrsument(instrument);
            else
                quantity = args.Amount; //use get request to get "free" amount of the asset available for trade

            PriceUpdate price = null;
            price = new PriceUpdate { Amount = args.Price };

            SpotOrderRequest request = new SpotOrderRequest();
            request.Quantity = quantity;
            request.Symbol = instrument.TickerSYM;
            request.Type = Binance.OrderType.MARKET;
            request.TimeStamp = args.Time;

           switch (type)
            {
                case ExchangeType.Buy:
                    request.Side = OrderSide.BUY;
                    break;
                case ExchangeType.Sell:
                    request.Side = OrderSide.SELL;
                    break;
            }

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("X-MBX-APIKEY", BrokerCredentials.APIKey);

            Dictionary<string, string> queries = new Dictionary<string, string>();
            string signature = GenerateSignature($"timestamp={DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}", BrokerCredentials.Secret);
            queries.Add("timestamp", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString());
            queries.Add("signature", signature);
            string requestURL = GenerateURL(PlatformArgs["ORDERSAPI"], queries);
            string requestJSON = JsonSerializer.Serialize(request);
            string responseJSON = await SendHTTPPost(requestURL, requestJSON, headers);

            SpotOrderResponse? response = JsonSerializer.Deserialize<SpotOrderResponse>(responseJSON);
            if (response == null)
            {
                ErrorResponse? errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseJSON);
                ErrorEvent errorEvent = new ErrorEvent()
                {
                    Date = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString(),
                    Name = "Error",
                    ID = "",
                    Action = "BinanceExchange;Spot",
                    Value = "",
                };
                if (errorResponse != null)
                {
                    errorEvent.Extras = errorResponse.Message.Replace(',', '>');
                    errorEvent.Value = errorResponse.Code.ToString();
                }
                else
                {
                    errorEvent.Value = "UnknownError";
                }
                await Logger.LogEvent(errorEvent);
                return;
            }

            LogEvent logEvent = new LogEvent()
            {
                Date = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString(),
                Name = "Exchange",
                Value = type.ToString(),
                Extras = $"BinanceExchange;{args.Instrument.TickerSYM};{args.Instrument.Name};{args.Instrument.InstrumentType};{args.Price};{args.Amount}{args.Time}"
            };
            await Logger.LogEvent(logEvent);
        }

        public override async Task<decimal> GetAvailableFunds()
        {
            //not required for binance
            throw new NotImplementedException();
        }

        public override async Task<List<FinancialInstrument>> GetAvailableInstruments()
        {
            List<FinancialInstrument> instruments = new List<FinancialInstrument>();

            AccountInfoResponse? accountInfoResponse = await GetAccountInfo();
            if (accountInfoResponse == null)
                return instruments;

            foreach (Balance balance in accountInfoResponse.Balances)
            {
                instruments.Add(new FinancialInstrument()
                {
                    InstrumentType = InstrumentType.Crypto,
                    Name = "",
                    TickerSYM = balance.Asset
                });
            }

            return instruments;
        }

        public override async Task<decimal> GetQuantityOfIntrsument(FinancialInstrument instrument)
        {
            AccountInfoResponse? accountInfoResponse = await GetAccountInfo();
            if (accountInfoResponse == null)
                return 0;

            Balance? assetBalance = accountInfoResponse.Balances.FirstOrDefault(x => x.Asset == instrument.TickerSYM);
            if (assetBalance == null)
                return 0;

            return assetBalance.Free;
        }

        public override async Task<bool> HasAvailableInstruments(FinancialInstrument instrument, decimal quantity)
        {
            AccountInfoResponse? accountInfoResponse = await GetAccountInfo();
            if (accountInfoResponse == null)
                return false;

            Balance? assetBalance = accountInfoResponse.Balances.FirstOrDefault(x => x.Asset == instrument.TickerSYM);
            if (assetBalance == null)
                return false;

            if (assetBalance.Free < quantity)
                return false;

            return true;
        }

        private string GenerateSignature(string query, string secret)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(secret);
            byte[] queryBytes = Encoding.UTF8.GetBytes(query);
            byte[] hash;

            using (HMACSHA256 hmac = new HMACSHA256(keyBytes))
                hash = hmac.ComputeHash(queryBytes);

            return BitConverter.ToString(hash).Replace("-","").ToLower();
        }

        private async Task<AccountInfoResponse?> GetAccountInfo()
        {
            AccountInfoResponse? response = new AccountInfoResponse();

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("X-MBX-APIKEY", BrokerCredentials.APIKey);

            Dictionary<string, string> queries = new Dictionary<string, string>();
            string signature = GenerateSignature($"timestamp={DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}", BrokerCredentials.Secret);
            queries.Add("timestamp", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString());
            queries.Add("signature", signature);
            string requestURL = GenerateURL(PlatformArgs["ACCOUNTINFO"], queries);

            string responseJSON = await SendHTTPGet(requestURL, headers);
            response = JsonSerializer.Deserialize<AccountInfoResponse>(responseJSON);

            if (response == null)
            {
                ErrorResponse? errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseJSON);
                ErrorEvent errorEvent = new ErrorEvent()
                {
                    Date = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString(),
                    Name = "Error",
                    ID = "",
                    Action = "BinanceExchange;AccountInfo",
                    Value = "",
                };
                if (errorResponse != null)
                {
                    errorEvent.Extras = errorResponse.Message.Replace(',', '>');
                    errorEvent.Value = errorResponse.Code.ToString();
                }
                else
                {
                    errorEvent.Value = "UnknownError";
                }
                await Logger.LogEvent(errorEvent);
            }

            return response;
        }
    }
}

