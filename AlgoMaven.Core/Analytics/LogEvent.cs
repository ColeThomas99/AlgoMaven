using System;
namespace AlgoMaven.Core.Analytics
{
    public class LogEvent
    {
        public string Date { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string ID { get; set; }
        public string Extras { get; set; }
    }
}

