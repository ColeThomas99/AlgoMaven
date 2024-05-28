using System;
namespace AlgoMaven.Core.Analytics
{
    public class PriceLogEvent : LogEvent
    {
        public string APIFetchStartTime { get; set; }
        public string APIFetchEndTime { get; set; }
        public string InstrumentName { get; set; }
    }
}

