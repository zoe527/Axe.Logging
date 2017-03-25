using System;
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
    }
}