using CommandLine;
using FoldersSync;
using FoldersSync.Models;

OptionsModel argsOptions = new OptionsModel();

await Parser.Default.ParseArguments<Options>(args).WithParsedAsync(async options =>
{
    if (ArgumentValidator.ValidateSrcFolderArg(options.SourcePath))
    { argsOptions.SourceFolderPath = options.SourcePath; }
    if (ArgumentValidator.ValidateRpcFolderArg(options.SourcePath, options.ReplicaPath))
    { argsOptions.ReplicaFolderPath = options.ReplicaPath; }
    if (ArgumentValidator.ValidateTimeArg(options.SyncTimePeriodMin))
    { argsOptions.SyncIntervalMinute = options.SyncTimePeriodMin; }
    if (ArgumentValidator.ValidateLogFolderArg(options.SourcePath, options.ReplicaPath,options.LogPath))
    { argsOptions.LogFileFolderPath = options.LogPath; }
}
);

Console.WriteLine(Constants.Header);

double syncIntervalMinute=argsOptions.SyncIntervalMinute;

//create log file 
string logFileGuid = await LogFileManager.CreateLogFile(argsOptions);
string logFilePathName = LogFileManager.GetLogFileName(argsOptions.LogFileFolderPath, logFileGuid);

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
    await PeriodicSynchronizer.RunSync(argsOptions, logFileGuid);
    using var timer = new PeriodicTimer(TimeSpan.FromMinutes(syncIntervalMinute));
    try
    {
        while (await timer.WaitForNextTickAsync(token))
        {
            await PeriodicSynchronizer.RunSync(argsOptions, logFileGuid);
        }
    }
    catch (OperationCanceledException)
    {
        Logger.WriteLogToConsoleLogfile(logFilePathName, Constants.ShutdownSignalReceived);
        Logger.WriteLogToConsoleLogfile(logFilePathName, Constants.ApplicationTerminated);
        Console.WriteLine(Constants.LogFileIsAt + logFilePathName);
    }
}
