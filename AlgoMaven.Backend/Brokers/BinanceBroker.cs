using System;
using AlgoMaven.Backend.Algorithms;
using AlgoMaven.Backend.Brokers.Binance.Requests;
using AlgoMaven.Backend.Enums;
using AlgoMaven.Backend.Models;
using AlgoMaven.Backend.Brokers.Binance;
using AlgoMaven.Backend.Brokers.Binance.Responses;
using System.Text.Json;

namespace AlgoMaven.Backend.Brokers
{
	public class BinanceBroker : BrokerBase
	{
		public BinanceBroker(ref UserAccount user ) : base(ref user)
		{
            Name = "Binance";
            URL = "https://www.binance.com";
            PlatformArgs.Add("ORDERSAPI", Path.Combine(URL, "/api/v3/order"));
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
            {
                quantity = GetQuantityOfIntrsument(instrument);
            }
            else
            {
                quantity = args.Amount; //use get request to get "free" amount of the asset available for trade
            }

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

            string requestJSON = JsonSerializer.Serialize(request);
            string responseJSON = await SendHTTPPost(PlatformArgs["ORDERSAPI"], requestJSON, null); //TODO - HACK - FIXME

            SpotOrderResponse? response = JsonSerializer.Deserialize<SpotOrderResponse>(responseJSON);
            if (response == null)
            {
                //deserialise error response instead
                ErrorResponse? errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseJSON);
                if (errorResponse != null)
                {
                    //log error event
                }
                else
                {
                    //log unknown error
                }
            }

            //send request
            //get response
            //check for errors
            //check if exchange type successful
            //resume
        }

        public override decimal GetAvailableFunds()
        {
            throw new NotImplementedException();
        }

        public override List<FinancialInstrument> GetAvailableInstruments()
        {
            throw new NotImplementedException();
        }

        public override decimal GetQuantityOfIntrsument(FinancialInstrument instrument)
        {
            throw new NotImplementedException();
        }

        public override bool HasAvailableInstruments(FinancialInstrument instrument, decimal quantity)
        {
            throw new NotImplementedException();
        }
    }
}

