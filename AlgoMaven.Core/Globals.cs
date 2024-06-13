using System;
using AlgoMaven.Core.Analytics;
using AlgoMaven.Core.Enums;
using AlgoMaven.Core.MarketData;
using AlgoMaven.Core.Models;

namespace AlgoMaven.Core
{
    public class Globals
    {
        public static List<FinancialInstrument> FinancialInstruments = new List<FinancialInstrument>();
        public static MarketDataAPIBase MarketDataAPI;
        public static List<MarketDataAPIBase> MarketAPIS = new List<MarketDataAPIBase>();
        public static Dictionary<string, List<(List<PriceUpdate>, InstrumentType, string, string)>> MarketAPIPrices = new Dictionary<string, List<(List<PriceUpdate>, InstrumentType, string, string)>>(); //= new Dictionary<string, List<PriceUpdate>>(); // tuple value <-- List<PriceUpdate> + InstrumentType + Instrument Symbol + Instrument Name
                                                                                                                                                                                                           //Consider making the key the instrument instead of the marketapi name (faster potentially)
        private static Dictionary<string, (InstrumentType, int)> marketAPIRankings = new Dictionary<string, (InstrumentType, int)>();
        public static Dictionary<string, (InstrumentType, int)> MarketAPIRankings
        {
            get { return marketAPIRankings; }
            set { marketAPIRankings = value; marketAPIRankings = marketAPIRankings.OrderBy(x => x.Value.Item2).ToDictionary(y => y.Key, y => y.Value); }
        }

        public Globals()
        {
            MarketDataAPI = new DummyMarketDataAPI();
            FinancialInstruments.AddRange(new FinancialInstrument[]
            {
                new FinancialInstrument { InstrumentType = InstrumentType.Crypto, Name = "Bitcoin", TickerSYM = "BTC" }
            });

            MarketAPIS.AddRange(new MarketDataAPIBase[]
            {
                new BinanceMarketDataAPI()
            });
        }

        public static void AddToMarketQueue(string key, MarketDataAPIBase api)
        {

        }

        public static void RemoveFromMarketQueue(string key)
        {

        }

        public static List<PriceUpdate?> Prices(int max)
        {
            List<PriceUpdate?> result = new List<PriceUpdate?>();

            return result;
        }

        public static async Task ManageMasterMarketQueue()
        {
            //while (true)
            //{
            //foreach (MarketDataAPIBase api in MarketAPIS)
            //{
            //get prices
            //store in variables

            //foreach (KeyValuePair<string, (List<PriceUpdate>, InstrumentType, string, string)> o in MarketAPIPrices)
            //{
            //o.Value.
            //}

            //check if marketAPI name exists in dictionary
            //update with getprices value
            //}

            Console.WriteLine(MarketAPIPrices.Count);

            foreach (string keyq in MarketAPIPrices.Keys)
                await Task.Run(() => ManageMarketQueue(keyq));
            //}
        }

        private static async Task ManageMarketQueue(string key)
        {

            foreach ((List<PriceUpdate>, InstrumentType, string, string) list in MarketAPIPrices[key])
            {
                Task.Run(() => ManageInstrumentQueue(key, list.Item2, list.Item4, list.Item3));
            }

            //CALL INSTRUMENT QUEUE FOREACH INSTRUMENT IN API INIT (KEY)
            /*
			 * every one minute:
			 *	call the getprices task <-- which instruments?
			 *	store the result in the prices dictionary
			 */
        }

        private static async Task InitInstrumentPrices(string key, FinancialInstrument instrument, string symbol, long end)
        {
            long start = DateTimeOffset.FromUnixTimeMilliseconds(end).AddMinutes(-100).ToUnixTimeMilliseconds();
            List<PriceUpdate>? prices = await MarketAPIS.First(x => x.Name == key).GetPricesAsync(instrument, start, end);
            lock (MarketAPIPrices)
            {
                MarketAPIPrices[key].First(x => x.Item3 == symbol).Item1.AddRange(prices);
            }
        }

        private static async Task ManageInstrumentQueue(string key, InstrumentType type, string name, string symbol)
        {
            long end, start;
#if DEBUG
            end = 1713394800000;
            start = DateTimeOffset.FromUnixTimeMilliseconds(end).AddMinutes(-1).ToUnixTimeMilliseconds();
#else
            end = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            start = DateTimeOffset.Now.AddMinutes(-1).ToUnixTimeMilliseconds();
#endif
            FinancialInstrument instrument = new FinancialInstrument() { Name = name, TickerSYM = symbol, InstrumentType = type };
            await InitInstrumentPrices(key, instrument, symbol, end);

            while (true)
            {
#if DEBUG
                end = DateTimeOffset.FromUnixTimeMilliseconds(end).AddMinutes(1).ToUnixTimeMilliseconds();
                start = DateTimeOffset.FromUnixTimeMilliseconds(end).AddMinutes(-1).ToUnixTimeMilliseconds();
#else
				start = DateTimeOffset.Now.AddMinutes(-1).ToUnixTimeMilliseconds();//
				end = DateTimeOffset.Now.ToUnixTimeMilliseconds();
#endif

                List<PriceUpdate>? prices = await MarketAPIS.First(x => x.Name == key).GetPricesAsync(instrument, start, end);

                PriceLogEvent priceLogEvent = new PriceLogEvent()
                {

                };

                if (Settings.EnableLogging)
                    Logger.LogEvent(priceLogEvent, prices);

                lock (MarketAPIPrices)
                {
                    MarketAPIPrices[key].First(x => x.Item3 == symbol).Item1.AddRange(prices); 
                }
#if DEBUG
                await Task.Delay(6000);
#else
				await Task.Delay(60000);
#endif
            }
        }
    }
}

