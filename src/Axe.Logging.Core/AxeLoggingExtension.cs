using System;
using System.Collections.Generic;
using System.Linq;
using Axe.Logging.Core.Entity;

namespace Axe.Logging.Core
{
    public static class AxeLoggingExtension
    {
        public static T Mark<T>(this T exception, LogEntry logEntry) where T : Exception
        {
            exception.Data.Add(logEntry.Level.ToString(), logEntry);
            return exception;
        }

        public static LogEntry[] GetLogEntry(this Exception exception, int maxLevel)
        {
            var logEntryList = new List<LogEntry>();

            var aggregateException = exception as AggregateException;
            if (aggregateException != null)
            {
                var aggregateExceptionInnerExceptions = aggregateException.InnerExceptions;

                logEntryList = aggregateExceptionInnerExceptions.Where(aggregateExceptionInnerException => aggregateExceptionInnerException != null).Aggregate(logEntryList, (current, aggregateExceptionInnerException) => current.ErgodicLogEntryList(maxLevel, aggregateExceptionInnerException));
            }
            else
            {
                logEntryList = logEntryList.ErgodicLogEntryList(maxLevel, exception);
            }

            return logEntryList.ToArray();
        }
    }
}