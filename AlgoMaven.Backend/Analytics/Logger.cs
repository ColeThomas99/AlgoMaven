using System;
namespace AlgoMaven.Backend.Analytics
{
	public class Logger
	{
		private const string DefaultFileName = "log.csv";

		public void LogEvent(LogEvent logEvent)
		{
			if (!File.Exists(DefaultFileName))
				CreateLogFile();

			using (StreamWriter streamWriter = new StreamWriter(DefaultFileName))
			{
				string data = $"{logEvent.Name},{logEvent.ID},{logEvent.Type},{logEvent.Value},{logEvent.Extras}";
				streamWriter.WriteLine(data);
			}
		}

		public void CreateLogFile(string name = DefaultFileName)
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

