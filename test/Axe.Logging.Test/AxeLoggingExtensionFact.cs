using System;
using Axe.Logging.Core;
using Axe.Logging.Core.Entity;
using Xunit;

namespace Axe.Logging.Test
{
    public class AxeLoggingExtensionFact
    {
        [Fact]
        public void Should_Mark_LogEntry_To_The_Exception_When_This_Exception_Is_Nomal_Exception()
        {
            var logId = Guid.NewGuid();
            var logHappenedTime = DateTime.UtcNow;
            var nullReferenceException = new NullReferenceException();
            var logEntry = new LogEntry
            {
                Id = logId,
                Time = logHappenedTime,
                Entry = "I am entry uri with post",
                User = new { Name = "I am Nancy" },
                Data = new { Message = "I am so Sad"},
                Level = LoggingLevel.IKnowItWillHappen
            };
            var exceptionWithLogEntry = nullReferenceException.Mark(logEntry);

            var myLogEntry = (LogEntry)exceptionWithLogEntry.Data[LoggingLevel.IKnowItWillHappen.ToString()];
            Assert.Equal(logId, myLogEntry.Id);
            Assert.Equal(logHappenedTime, myLogEntry.Time);
            Assert.Equal("I am entry uri with post", myLogEntry.Entry);
            Assert.Equal("{ Name = I am Nancy }", myLogEntry.User.ToString());
            Assert.Equal("{ Message = I am so Sad }", myLogEntry.Data.ToString());
            Assert.Equal(LoggingLevel.IKnowItWillHappen, myLogEntry.Level);
        }

        [Fact]
        public void Should_Mark_LogEntry_To_The_Exception_When_This_Exception_Is_UnNomal_Exception()
        {
            var logId = Guid.NewGuid();
            var logHappenedTime = DateTime.UtcNow;
            var aggregateException = new AggregateException();
            var logEntry = new LogEntry
            {
                Id = logId,
                Time = logHappenedTime,
                Entry = "I am entry uri with post",
                User = new { Name = "I am Nancy" },
                Data = new { Message = "I am so so Sad" },
                Level = LoggingLevel.UnKnown
            };
            var exceptionWithLogEntry = aggregateException.Mark(logEntry);

            var myLogEntry = (LogEntry)exceptionWithLogEntry.Data[LoggingLevel.UnKnown.ToString()];
            Assert.Equal(logId, myLogEntry.Id);
            Assert.Equal(logHappenedTime, myLogEntry.Time);
            Assert.Equal("I am entry uri with post", myLogEntry.Entry);
            Assert.Equal("{ Name = I am Nancy }", myLogEntry.User.ToString());
            Assert.Equal("{ Message = I am so so Sad }", myLogEntry.Data.ToString());
            Assert.Equal(LoggingLevel.UnKnown, myLogEntry.Level);
        }
    }
}