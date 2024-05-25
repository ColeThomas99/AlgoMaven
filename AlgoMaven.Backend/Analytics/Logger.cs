using System;
using AlgoMaven.Backend.Models;

namespace AlgoMaven.Backend.Analytics
{
	public class Logger
	{
		private const string DefaultFileName = "log.csv";
		private const string PricesFileName = "prices.csv";
		private const string ErrorFileName = "errors.csv";

		public static async Task LogEvent(string? data = null, string file = DefaultFileName)
		{
			if (!File.Exists(file))
				await CreateLogFile();
			try
			{
				using (StreamWriter streamWriter = File.AppendText(file))
					streamWriter.WriteLine(data);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		public static async Task LogEvent(LogEvent logEvent)
		{
            string data = $"{logEvent.Date},{logEvent.Name},{logEvent.ID},{logEvent.Value},{logEvent.Extras}";
            await LogEvent(data);
		}

		public static async Task LogEvent(PriceLogEvent priceLogEvent, List<PriceUpdate> prices)
		{
            priceLogEvent.Extras = string.Join(';', prices.Select(x => x.Amount));
			string data = $"{priceLogEvent.Date},{priceLogEvent.APIFetchStartTime},{priceLogEvent.APIFetchEndTime},{priceLogEvent.Name},{priceLogEvent.ID},{priceLogEvent.InstrumentName},{priceLogEvent.Value},{priceLogEvent.Extras}";
			await LogEvent(data, PricesFileName);
		}

		public static async Task LogEvent(ErrorEvent errorEvent)
		{
            string data = $"{errorEvent.Date},{errorEvent.Name},{errorEvent.ID},{errorEvent.Value},{errorEvent.Action},{errorEvent.Extras}";
			await LogEvent(data, ErrorFileName);
        }

		public static async Task CreateLogFile(string name = DefaultFileName)
		{
			try
			{
				File.Create(name);
			}
			catch
			{
				//error handling todo
			}
		}
	}
}

