using System;
namespace AlgoMaven.Backend.Analytics
{
	public class LogEvent
	{
		public string Name { get; set; }
		public string Type { get; set; }
		public string Value { get; set; }
		public string ID { get; set; }
		public string Extras { get; set; }
	}
}

