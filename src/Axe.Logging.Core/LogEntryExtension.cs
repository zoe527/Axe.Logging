using System;
using System.Collections.Generic;
using Axe.Logging.Core.Entity;

namespace Axe.Logging.Core
{
    public static class LogEntryListExtension
    {
        public static List<LogEntry> ErgodicLogEntryList(this List<LogEntry> logEntryList, int maxLevel, Exception exceptionInnerException)
        {
            for (var i = 0; i < maxLevel; i++)
            {
                if (exceptionInnerException != null)
                {
                    var myException = exceptionInnerException.InnerException;
                    logEntryList = logEntryList.BuildLogEntryList(myException);
                    exceptionInnerException = myException;
                }
            }
            return logEntryList;
        }

        public static List<LogEntry> BuildLogEntryList(this List<LogEntry> logEntryList, Exception myException)
        {
            if (myException != null)
            {
                if (myException.Data[LoggingLevel.IKnowItWillHappen.ToString()] != null)
                {
                    logEntryList.Add((LogEntry)myException.Data[LoggingLevel.IKnowItWillHappen.ToString()]);
                }
                if (myException.Data[LoggingLevel.UnKnown.ToString()] != null)
                {
                    logEntryList.Add((LogEntry)myException.Data[LoggingLevel.UnKnown.ToString()]);
                }
            }
            return logEntryList;
        }
    }
}