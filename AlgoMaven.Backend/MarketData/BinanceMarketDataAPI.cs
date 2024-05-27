using System;
using AlgoMaven.Backend.Models;
using AlgoMaven.Backend.MarketData.Binance;
//using AlgoMaven.Backend.MarketData.Binance;
//using AlgoMaven.Backend.Models;
using System.Text.Json;
using AlgoMaven.Backend.Enums;
using System.Collections.Generic;
using AlgoMaven.Backend.MarketData.Binance.Requests;
//using AlgoMaven.Backend.MarketData.Binance;
using AlgoMaven.Backend.MarketData.Binance.Responses;

namespace AlgoMaven.Backend.MarketData
{
	public class BinanceMarketDataAPI : MarketDataAPIBase
	{
		public BinanceMarketDataAPI()
		{
            APIEndPoint = "https://api.binance.com/";
            //api/v1/exchangeInfo
            Name = "Binance";
		}

        public override async Task InitMarketInstruments()
        {
            Globals.MarketAPIRankings.Add(Name, (InstrumentType.Crypto, 1));
            BinanceInstrumentListRequest request = new BinanceInstrumentListRequest();

            string uri = CraftInstrumentListRequest(request);
            string result = await GetAsync(uri);

            ProcessInstrumentListResponse(result);
        }

        public override async Task<List<PriceUpdate>?> GetPricesAsync(FinancialInstrument instrument, long timeStampSeconds, long? endTimeStampSeconds, string? interval = null, int limit = 100)
        {
            List<PriceUpdate>? prices = new List<PriceUpdate>();

            //https://api.binance.com/api/v3/klines?symbol=BTCUSDT&interval=1m&limit=1000

            BinanceMarketPriceRequest request = new BinanceMarketPriceRequest();
            request.Symbol = instrument.TickerSYM;
            request.StartTime = timeStampSeconds;
            request.EndTime = endTimeStampSeconds;
            request.Limit = limit;
            request.Interval = BinanceMarketIntervals.OneMinute; //sort this out with interval

            string uri = CraftPriceRequest(request);
            string result = await GetAsync(uri);

            prices = ProcessPriceResponse(result);

            return prices;
        }

        private void ProcessInstrumentListResponse(string result)
        {
            BinanceInstrumentListResponse? response = JsonSerializer.Deserialize<BinanceInstrumentListResponse>(result);

            if (response == null)
                return;

            int count = 0;
            foreach (BinanceSymbolInfo info in response.Symbols)
            {
                if (count == 5)
                    break;

                lock (Globals.MarketAPIPrices)
                {
                    if (info.QuoteAsset == "USDT")
                    {
                        Console.WriteLine($"{info.BaseAsset} : {info.QuoteAsset}");
                        //add base asset to main list
                        if (Globals.MarketAPIPrices.ContainsKey(Name))
                        {
                            //public static
                            //Dictionary<string, List<(List<PriceUpdate>, InstrumentType, string, string)>> MarketAPIPrices =
                            //new Dictionary<string, List<(List<PriceUpdate>, InstrumentType, string, string)>>();
                            //
                            Globals.MarketAPIPrices[Name].Add(new(new List<PriceUpdate>(), InstrumentType.Crypto, info.BaseAsset, ""));
                        }
                        else
                        {
                            Globals.MarketAPIPrices.Add(Name, new List<(List<PriceUpdate>, InstrumentType, string, string)>());
                            Globals.MarketAPIPrices[Name].Add(new(new List<PriceUpdate>(), InstrumentType.Crypto, info.BaseAsset, ""));
                        }
                        count++;
                    }
                    //count++;
                }
            }
            Globals.MarketAPIPrices[Name].Add(new(new List<PriceUpdate>(), InstrumentType.Crypto, "SSV", ""));
        }

        private List<PriceUpdate>? ProcessPriceResponse(string result)
        {
            BinanceMarketPriceResponse response = new BinanceMarketPriceResponse();
            response.Candles = new List<BinanceMarketCandle>();

            List<PriceUpdate>? prices = new List<PriceUpdate>();

            List<List<object>>? value = JsonSerializer.Deserialize<List<List<object>>>(result);

            if (value == null)
                return null;

            try
            {
                foreach (List<object> entry in value)
                {
                    response.Candles.Add(new BinanceMarketCandle()
                    {
                        OpenTime = long.Parse(entry[0].ToString()),
                        Open = decimal.Parse(entry[1].ToString()),
                        High = decimal.Parse(entry[2].ToString()),
                        Low = decimal.Parse(entry[3].ToString()),
                        Close = decimal.Parse(entry[4].ToString()),
                        Volume = decimal.Parse(entry[5].ToString()),
                        CloseTime = long.Parse(entry[6].ToString()),
                        BaseAssetVolume = decimal.Parse(entry[7].ToString()),
                        TradeCount = int.Parse(entry[8].ToString()),
                        TakerBuyVolume = decimal.Parse(entry[9].ToString()),
                        TakeBuyBaseAssetVolume = decimal.Parse(entry[10].ToString()),
                        Ignore = decimal.Parse(entry[11].ToString())
                    });

                    prices.Add(new PriceUpdate
                    {
                        Amount = decimal.Parse(entry[4].ToString()),
                        Time = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(entry[6].ToString())),
                        Instrument = new FinancialInstrument { TickerSYM = "" }
                    });
                }
            }
            catch
            {
                return null;
            }

            if (prices.Count == 0)
                prices = null;

            return prices;
        }

        private async Task<string> GetAsync(string uri)
        {
            string result = String.Empty;

            HttpClient client = new HttpClient();
            using HttpResponseMessage response = await client.GetAsync(uri);
            result = await response.Content.ReadAsStringAsync();

            response.Dispose();
            client.Dispose();
            return result;
        }

        private string CraftInstrumentListRequest(BinanceInstrumentListRequest request)
        {
            string result = String.Empty;

            string endPoint = Path.Combine(APIEndPoint, "api/v3/exchangeInfo"); //+ "?";

            return endPoint;
        }

        private string CraftPriceRequest(BinanceMarketPriceRequest request)
        {
            string result = String.Empty;
            string interval = String.Empty;

            switch (request.Interval)
            {
                case BinanceMarketIntervals.OneMinute:
                    interval = "1m";
                    break;
                case BinanceMarketIntervals.ThreeMinutes:
                    interval = "3m";
                    break;
                case BinanceMarketIntervals.FiveMinutes:
                    interval = "5m";
                    break;
                case BinanceMarketIntervals.OneHour:
                    interval = "1h";
                    break;
                case BinanceMarketIntervals.TwoHours:
                    interval = "2h";
                    break;
                case BinanceMarketIntervals.FourHours:
                    interval = "5h";
                    break;
                case BinanceMarketIntervals.OneDay:
                    interval = "1d";
                    break;
                case BinanceMarketIntervals.ThreeDays:
                    interval = "3d";
                    break;
                case BinanceMarketIntervals.OneWeek:
                    interval = "1w";
                    break;
                case BinanceMarketIntervals.OneMonth:
                    interval = "1M";
                    break;
            }

            List<URIArg> args = new List<URIArg>();
            args.Add(new URIArg() { Name = "symbol", Value = $"{request.Symbol}USDT" });
            args.Add(new URIArg() { Name = "startTime", Value = request.StartTime?.ToString() });
            args.Add(new URIArg() { Name = "endTime", Value = request.EndTime?.ToString()});
            args.Add(new URIArg() { Name = "interval", Value = interval });
            args.Add(new URIArg() { Name = "limit", Value = request.Limit?.ToString()});

            string endPoint = Path.Combine(APIEndPoint, "api/v3/klines") + "?";

            foreach (URIArg arg in args)
                endPoint += $"{arg.Name}={arg.Value}&";

            endPoint = endPoint.TrimEnd('&');

            return endPoint;
        }
    }
}

