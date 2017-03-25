using System;
using System.Collections.Generic;
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

                foreach (var aggregateExceptionInnerException in aggregateExceptionInnerExceptions)
                {
                        if (aggregateExceptionInnerException != null)
                        {
                            var exceptionInnerException = aggregateExceptionInnerException;
                            for (var i = 0; i < maxLevel; i++)
                            {
                                if (exceptionInnerException != null)
                                {
                                    var myException = exceptionInnerException.InnerException;
                                    if (myException != null && myException.Data[LoggingLevel.IKnowItWillHappen.ToString()] != null)
                                    {
                                        logEntryList.Add((LogEntry)myException.Data[LoggingLevel.IKnowItWillHappen.ToString()]);
                                    }

                                    if (myException != null && myException.Data[LoggingLevel.UnKnown.ToString()] != null)
                                    {
                                        logEntryList.Add((LogEntry)myException.Data[LoggingLevel.UnKnown.ToString()]);
                                    }
                                    exceptionInnerException = myException;
                                }
                            }
                    }
                }
            }
            else
            {
                for (var i = 0; i < maxLevel; i++)
                {
                    if (exception != null)
                    {
                        var myException = exception.InnerException;
                        if (myException != null && myException.Data[LoggingLevel.IKnowItWillHappen.ToString()] != null)
                        {
                            logEntryList.Add((LogEntry)myException.Data[LoggingLevel.IKnowItWillHappen.ToString()]);
                        }
                        exception = myException;
                    }
                }
            }

            return logEntryList.ToArray();
        }
    }
}