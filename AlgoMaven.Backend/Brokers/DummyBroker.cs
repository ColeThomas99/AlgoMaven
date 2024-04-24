﻿using System;
using AlgoMaven.Backend.Models;
using AlgoMaven.Backend.Enums;
using AlgoMaven.Backend.MarketData;
using AlgoMaven.Backend.Algorithms;
using System.Linq;

namespace AlgoMaven.Backend.Brokers
{
    /// <summary>
    /// This is a 'Test' broker platform used to simulate the processes of a real brokerage..
    /// </summary>
	public class DummyBroker : BrokerBase
	{
		DummyMarketDataAPI marketDataAPI;
		public DummyBroker(ref UserAccount user) : base(ref user)
		{
			marketDataAPI = new DummyMarketDataAPI();
			Name = "Dummy";
            //MarketAPIRankings.Add(Globals.MarketAPIS[0].Name, (InstrumentType.Crypto, 1));

			//if (User.BrokerPlatforms.FirstOrDefault(x => x.Name == Name) == null)
				//throw new Exception($"Broker {Name} does not exist for user.");
		}

        public override void AddInstrument(FinancialInstrument instrument, decimal quantity)
        {
            if (Wallet.UserInstruments.FirstOrDefault(x => x.Instrument.Name == instrument.Name && x.Instrument.TickerSYM == instrument.TickerSYM) == null)
            {
                UserFinancialInstrument userInstruments = new UserFinancialInstrument()
                {
                    Instrument = instrument,
                    Quantity = quantity
                };
                Wallet.UserInstruments.Add(userInstruments);
            }
            else
            {
                Wallet.UserInstruments.First(x => x.Instrument.Name == instrument.Name && x.Instrument.TickerSYM == instrument.TickerSYM).Quantity += quantity;
            }
        }

        public override decimal GetAvailableFunds()
        {
			return Wallet.AvailableFunds;
        }

        public override bool HasAvailableInstruments(FinancialInstrument instrument, decimal quantity)
        {
            if (Wallet.UserInstruments.FirstOrDefault(x => x.Instrument.TickerSYM == instrument.TickerSYM) == null)
                return false;
            if (Wallet.UserInstruments.First(x => x.Instrument.TickerSYM == instrument.TickerSYM).Quantity < quantity)
                return false;

            return true;
        }

        public override async Task Exchange(ExchangeType type, TradeSignalArgs args)
        {
            FinancialInstrument instrument = args.Instrument;
            decimal quantity = 0;
            if (args.SellAll)
            {
                quantity = GetQuantityOfIntrsument(args.Instrument);
            }
            else
            {
                quantity = args.Amount;
            }

            Console.WriteLine("Exchanged:");
            PriceUpdate price = null;//marketDataAPI.GetPrices(instrument, DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            price = new PriceUpdate { Amount = args.Price };
            switch (type)
			{
				case ExchangeType.Buy:
					if (GetAvailableFunds() >= price.Amount * quantity)
					{
						Wallet.AvailableFunds -= price.Amount * quantity;
						AddInstrument(instrument, quantity);
                        Console.WriteLine($"** Buy: {instrument.Name}, {quantity} @ {args.Price * quantity} @ {DateTimeOffset.FromUnixTimeMilliseconds(args.Time)}");
                        Console.WriteLine($"** Wallet Amount {Wallet.AvailableFunds}");
					}
					break;
				case ExchangeType.Sell:
                    if (HasAvailableInstruments(instrument, quantity))
                    {
                        Wallet.UserInstruments.First(x => x.Instrument.TickerSYM == instrument.TickerSYM).Quantity -= quantity;
                        Wallet.AvailableFunds += price.Amount * quantity;
                        Console.WriteLine($"** Sell: {instrument.Name}, {quantity} @ {args.Price * quantity} @ {DateTimeOffset.FromUnixTimeMilliseconds(args.Time)}");
                        Console.WriteLine($"** Wallet Amount {Wallet.AvailableFunds}");
                    }
					break;
			}
        }

        public override List<FinancialInstrument> GetAvailableInstruments()
        {
            List<FinancialInstrument> result = new List<FinancialInstrument>();

            result.AddRange(new FinancialInstrument[]
            {
                new FinancialInstrument { InstrumentType = InstrumentType.Crypto, Name = "Bitcoin", TickerSYM = "BTC" }
            });

            return result;
        }

        public override decimal GetQuantityOfIntrsument(FinancialInstrument instrument)
        {
            decimal result = 0;
            UserFinancialInstrument? userInstrument = Wallet.UserInstruments.FirstOrDefault(x => x.Instrument.Name == instrument.Name && x.Instrument.TickerSYM == instrument.TickerSYM);
            if (userInstrument == null)
                return 0;

            result = userInstrument.Quantity;
            return result;
        }
    }
}

