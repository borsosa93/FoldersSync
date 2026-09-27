using System;
using System.Collections.Generic;
using System.Text;

namespace FoldersSync
{
    public class Logger
    {
        public static async Task WriteLogToConsoleLogfile(string logFilePathName, string logMessage)
        {
            string timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ");
            string timeMessageToLog=timestamp + " " + logMessage;
            using StreamWriter sw = File.AppendText(logFilePathName);
            {
                await sw.WriteLineAsync(timeMessageToLog);
            }
            Console.WriteLine(timeMessageToLog);
        }
    }
}
