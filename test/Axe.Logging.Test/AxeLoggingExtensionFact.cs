using System;
using System.Collections.Generic;
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

        [Fact]
        public void Should_Get_Empty_When_The_Excpetion_Is_Nomal_And_Current_Happened_Level_Is_More_Than_MaxLevel()
        {
            var logId = Guid.NewGuid();
            var logHappenedTime = DateTime.UtcNow;
            var logEntry = new LogEntry
            {
                Id = logId,
                Time = logHappenedTime,
                Entry = "I am entry uri with post",
                User = new { Name = "I am Nancy" },
                Data = new { Message = "I am so so Sad When I can not entry" },
                Level = LoggingLevel.IKnowItWillHappen
            };

            const int maxLevel = 1;
            var timeoutException = new TimeoutException(
                "Timed out",
                new ArgumentException("ID missing", new ArgumentNullException($"ID is Null").Mark(logEntry)));
            var logEntryList = timeoutException.GetLogEntry(maxLevel);
            Assert.Equal(0, logEntryList.Length);
        }

        [Fact]
        public void Should_Get_Empty_When_The_Excpetion_Is_Nomal_And_Current_Happened_Level_Is_Less_Than_MaxLevel()
        {
            var logId = Guid.NewGuid();
            var logHappenedTime = DateTime.UtcNow;
            var logEntry = new LogEntry
            {
                Id = logId,
                Time = logHappenedTime,
                Entry = "I am entry uri with post",
                User = new { Name = "I am Nancy" },
                Data = new { Message = "I am so so Sad When I can not entry" },
                Level = LoggingLevel.IKnowItWillHappen
            };

            const int maxLevel = 4;
            var timeoutException = new TimeoutException(
                "Timed out",
                new ArgumentException("ID missing", new ArgumentNullException($"ID", "ID is Null").Mark(logEntry)));
            var logEntryList = timeoutException.GetLogEntry(maxLevel);
            Assert.Equal(1, logEntryList.Length);
            var myLogEntry = logEntryList[0];
            Assert.Equal(logId, myLogEntry.Id);
            Assert.Equal(logHappenedTime, myLogEntry.Time);
            Assert.Equal("I am entry uri with post", myLogEntry.Entry);
            Assert.Equal("{ Name = I am Nancy }", myLogEntry.User.ToString());
            Assert.Equal("{ Message = I am so so Sad When I can not entry }", myLogEntry.Data.ToString());
            Assert.Equal(LoggingLevel.IKnowItWillHappen, myLogEntry.Level);
        }

         [Fact]
        public void Should_Get_Empty_When_The_Excpetion_Is_UnNomal_And_Current_Happened_Level_Is_More_Than_MaxLevel()
        {
            var logId = Guid.NewGuid();
            var logHappenedTime = DateTime.UtcNow;
            var logEntryForWarining = new LogEntry
            {
                Id = logId,
                Time = logHappenedTime,
                Entry = "I am entry uri with post",
                User = new { Name = "I am Nancy" },
                Data = new { Message = "I am so so Sad" },
                Level = LoggingLevel.IKnowItWillHappen
            };

            var logEntryForError = new LogEntry
            {
                Id = logId,
                Time = logHappenedTime,
                Entry = "I am entry uri with post",
                User = new { Name = "I am Nancy" },
                Data = new { Message = "I am so so Sad" },
                Level = LoggingLevel.IKnowItWillHappen
            };

            const int maxLevel = 1;
            var exceptions = new List<Exception>
            {
                new TimeoutException(
                    "Timed out",
                    new ArgumentException("ID missing", new ArgumentNullException($"ID", "ID is Null").Mark(logEntryForWarining))),
                new TimeoutException(
                    "Somethings not implemented",
                    new ArgumentException("Something happened", new NotImplementedException().Mark(logEntryForError)))
            };
            var aggregateException = new AggregateException(exceptions);

            var logEntryList = aggregateException.GetLogEntry(maxLevel);
            Assert.Equal(0, logEntryList.Length);
        }

        [Fact]
        public void Should_Get_Empty_When_The_Excpetion_Is_UnNomal_And_Current_Happened_Level_Is_Less_Than_MaxLevel()
        {
            var logId = Guid.NewGuid();
            var logHappenedTime = DateTime.UtcNow;
            var logEntryForWarining = new LogEntry
            {
                Id = logId,
                Time = logHappenedTime,
                Entry = "I am entry uri with post",
                User = new { Name = "I am Nancy" },
                Data = new { Message = "I am so so Sad" },
                Level = LoggingLevel.IKnowItWillHappen
            };

            var logEntryForError = new LogEntry
            {
                Id = logId,
                Time = logHappenedTime,
                Entry = "I am entry uri with Get",
                User = new { Name = "I am Rebecca" },
                Data = new { Message = "I am so so Happy" },
                Level = LoggingLevel.UnKnown
            };

            const int maxLevel = 4;
            var exceptions = new List<Exception>
            {
                new TimeoutException(
                    "Timed out",
                    new ArgumentException("ID missing", new ArgumentNullException($"ID", "ID is Null").Mark(logEntryForWarining))),
                new TimeoutException(
                    "Somethings not implemented",
                    new ArgumentException("Something happened", new NotImplementedException().Mark(logEntryForError)))
            };
            var aggregateException = new AggregateException(exceptions);

            var logEntryList = aggregateException.GetLogEntry(maxLevel);
            Assert.Equal(2, logEntryList.Length);

            var myLogEntryForWarining = logEntryList[0];
            Assert.Equal(logId, myLogEntryForWarining.Id);
            Assert.Equal(logHappenedTime, myLogEntryForWarining.Time);
            Assert.Equal("I am entry uri with post", myLogEntryForWarining.Entry);
            Assert.Equal("{ Name = I am Nancy }", myLogEntryForWarining.User.ToString());
            Assert.Equal("{ Message = I am so so Sad }", myLogEntryForWarining.Data.ToString());
            Assert.Equal(LoggingLevel.IKnowItWillHappen, myLogEntryForWarining.Level);

            var myLogEntryForError = logEntryList[1];
            Assert.Equal(logId, myLogEntryForError.Id);
            Assert.Equal(logHappenedTime, myLogEntryForError.Time);
            Assert.Equal("I am entry uri with Get", myLogEntryForError.Entry);
            Assert.Equal("{ Name = I am Rebecca }", myLogEntryForError.User.ToString());
            Assert.Equal("{ Message = I am so so Happy }", myLogEntryForError.Data.ToString());
            Assert.Equal(LoggingLevel.UnKnown, myLogEntryForError.Level);
        }
    }
}