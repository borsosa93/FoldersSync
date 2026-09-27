using FoldersSync;
using FoldersSync.Models;

Console.WriteLine(Constants.Header);

//get user input
UserInputModel userInput= InputDataReader.ReadInputParams();
double syncIntervalMinute=userInput.SyncIntervalMinute;

//create log file 
string logFileGuid = await LogFileManager.CreateLogFile(userInput);
string logFilePathName = LogFileManager.GetLogFileName(userInput.LogFileFolderPath, logFileGuid);

//create stop key
using var cts = new CancellationTokenSource();
Task timerTask = RunTimerAsync(cts.Token);
while (Console.ReadKey(true).Key != ConsoleKey.Escape) { }
Logger.WriteLogToConsoleLogfile(logFilePathName, Constants.ShuttingDownApplication);

//press stop key
cts.Cancel();
try
{
    await timerTask;
}
catch (OperationCanceledException)
{
    Logger.WriteLogToConsoleLogfile(logFilePathName, Constants.TimerStopped);
}

//run periodically until stop is pressed
async Task RunTimerAsync(CancellationToken token)
{
    using var timer = new PeriodicTimer(TimeSpan.FromMinutes(syncIntervalMinute));
    try
    {
        while (await timer.WaitForNextTickAsync(token))
        {
            await PeriodicSynchronizer.RunSync(userInput, logFileGuid);
        }
    }
    catch (OperationCanceledException)
    {
        Logger.WriteLogToConsoleLogfile(logFilePathName, Constants.ShutdownSignalReceived);
        Logger.WriteLogToConsoleLogfile(logFilePathName, Constants.ApplicationTerminated);
        Console.WriteLine(Constants.LogFileIsAt + logFilePathName);
    }
}
