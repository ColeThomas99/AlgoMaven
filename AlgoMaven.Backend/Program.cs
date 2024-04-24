using System;
using AlgoMaven.Backend.Algorithms;
using AlgoMaven.Backend.Bots;
using AlgoMaven.Backend.Brokers;
using AlgoMaven.Backend.Enums;
using AlgoMaven.Backend.MarketData;
using AlgoMaven.Backend.Models;

namespace AlgoMaven.Backend;

public class Program
{
    public async static Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        //builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            //app.UseSwagger();
            //app.UseSwaggerUI();
        }

        Globals globals = new Globals();

        //Globals.MarketAPIS.Add(new BinanceMarketDataAPI());

        foreach (MarketDataAPIBase marketDataAPI in Globals.MarketAPIS)
            await marketDataAPI.InitMarketInstruments();

        await Task.Run(() => Globals.ManageMasterMarketQueue());

        UserAccount user = new UserAccount();
        user.BrokerPlatforms.Add(new DummyBroker(ref user));
        user.BrokerPlatforms[0].Wallet.AvailableFunds = 1000;
        user.BrokerPlatforms[0].Wallet.UserInstruments = new List<UserFinancialInstrument>();
        user.BrokerPlatforms[0].Wallet.UserInstruments.Add(new UserFinancialInstrument { Instrument = new FinancialInstrument { InstrumentType = InstrumentType.Crypto, Name = "Bitcoin", TickerSYM = "BTC" }, Quantity = 0 });
        user.BrokerPlatforms[0].Wallet.UserInstruments.Add(new UserFinancialInstrument { Instrument = new FinancialInstrument { InstrumentType = InstrumentType.Crypto, Name = "Eth Coin", TickerSYM = "ETH" }, Quantity = 0 });
        user.BrokerPlatforms[0].Wallet.UserInstruments.Add(new UserFinancialInstrument { Instrument = new FinancialInstrument { InstrumentType = InstrumentType.Crypto, Name = "BNB Coin", TickerSYM = "BNB" }, Quantity = 0 });

        await Task.Delay(3000);
        /*StandardBot bot = new
            StandardBot(
            new AligatorAlgo(
                new FinancialInstrument
                {
                    InstrumentType = InstrumentType.Crypto, Name = "Bitcoin", TickerSYM = "BTC"
                }),
            user,
            new FinancialInstrument
            { InstrumentType = InstrumentType.Crypto, Name = "Bitcoin", TickerSYM = "BTC" }
            , user.BrokerPlatforms[0]);*/

        StandardBot bot = new StandardBot();
        bot.Algorithm = new AligatorAlgo(user.BrokerPlatforms[0].Wallet.UserInstruments[0].Instrument);
       // bot.Instrument = user.BrokerPlatforms[0].Wallet.UserInstruments[2].Instrument;
        bot.Broker = user.BrokerPlatforms[0];
        bot.User = user;
        bot.Options.BuyIncrement = (decimal)0.004;
        bot.Options.MaxBuyBeforeSell = 6;
        bot.Algorithm.Options = bot.Options;
        await bot.Run();

        BinanceMarketDataAPI m = new BinanceMarketDataAPI();
        List<PriceUpdate>? p = await m.GetPricesAsync(new FinancialInstrument { TickerSYM = "BTC" }, 1713139200000, 1713139900000, "1m", 5);
        foreach (PriceUpdate pp in p)
        {
            Console.WriteLine($"{pp.Amount} @ {pp.Time}");
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();

        
    }
}

