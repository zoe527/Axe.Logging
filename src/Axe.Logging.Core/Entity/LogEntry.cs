using System;

namespace Axe.Logging.Core.Entity
{
    [Serializable]
    public class LogEntry
    {
        public Guid Id { get; set; }
        public DateTime Time { get; set; }
        public string Entry { get; set; }
        public object User { get; set; }
        public object Data { get; set; }
        public LoggingLevel Level { get; set; }
    }
}