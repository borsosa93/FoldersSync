using FoldersSync.Models;

namespace FoldersSync
{
    public static class LogFileManager
    {        
        private static string _logFileNamePrefix = Constants.LogFileNamePrefix;
        public static async Task<string> CreateLogFile(UserInputModel userInputModel)
        {
            string logFilePath = userInputModel.LogFileFolderPath;
            string sourceFolderPath=userInputModel.SourceFolderPath;
            string replicaFolderPath=userInputModel.ReplicaFolderPath;
            double syncIntervalMinute = userInputModel.SyncIntervalMinute;
            string logFileGuid = Guid.NewGuid().ToString();
            string logFilePathName = GetLogFileName(logFilePath, logFileGuid);

            File.AppendAllText(logFilePathName, sourceFolderPath + Environment.NewLine);
            File.AppendAllText(logFilePathName, replicaFolderPath + Environment.NewLine);
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(Constants.SyncStartsIn+syncIntervalMinute+" min");

            return logFileGuid;
        }

        public static string GetLogFileName(string filePath, string fileNameGuid)
        {
            return (filePath + "\\" + _logFileNamePrefix + fileNameGuid + ".txt");
        }

    }
}
