using FoldersSync.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoldersSync
{
    public class PeriodicSynchronizer
    {
        public static async Task RunSync(OptionsModel userInput, string logFileGuid)
        {
            string sourceFolderPath = userInput.SourceFolderPath;
            string replicaFolderPath = userInput.ReplicaFolderPath;
            string logFileFolderPath = userInput.LogFileFolderPath;
            double syncIntervalMinute = userInput.SyncIntervalMinute;
            string replicaFolderParentPath = Directory.GetParent(replicaFolderPath).ToString();
            string logFilePathName = LogFileManager.GetLogFileName(logFileFolderPath, logFileGuid);
            string syncedReplicaFolderPath = null;

            Logger.WriteLogToConsoleLogfile(logFilePathName, Constants.SyncStarted);

            if (Directory.Exists(replicaFolderPath + "Backup"))
                {
                Directory.Delete(replicaFolderPath + "Backup", true);
                }

            //collect the content of source folder
            var contentInSourceFolder = FolderProcessor.MapFolderContent(sourceFolderPath);
            List<string> sourceFiles = contentInSourceFolder.files;
            List<string> sourceSubfolders = contentInSourceFolder.subfolders;
            List<FileDataModel> sourceFilesData = Helpers.GetFilesData(sourceFiles, sourceFolderPath);
            List<SubfolderDataModel> sourceSubfoldersData = Helpers.GetSubfoldersData(sourceSubfolders, sourceFolderPath);

            //collect the content of replica folder
            var contentInReplicaFolder = FolderProcessor.MapFolderContent(replicaFolderPath);
            List<string> replicaFiles = contentInReplicaFolder.files;
            List<string> replicaSubfolders = contentInReplicaFolder.subfolders;
            List<FileDataModel> replicaFilesData = Helpers.GetFilesData(replicaFiles, replicaFolderPath);
            List<SubfolderDataModel> replicaSubfoldersData = Helpers.GetSubfoldersData(replicaSubfolders, replicaFolderPath);

            //create a folder called tmp-<guid> in the parent folder of replica
            string tmpFolderPath = FolderProcessor.CreateTemporaryFolder(replicaFolderParentPath, logFileGuid);

            //SyncReplicaFiles
            FilesFoldersContentSynchronizer.SyncReplicaFiles(sourceFilesData, replicaFilesData, logFilePathName);

            //SyncReplicaSubfolders
            FilesFoldersContentSynchronizer.SyncReplicaSubfolders(sourceSubfoldersData, replicaSubfoldersData, logFilePathName);

            //SyncSourceSubfolders
            FilesFoldersContentSynchronizer.SyncSourceSubfolders(sourceSubfoldersData, replicaSubfoldersData, tmpFolderPath, logFilePathName);

            //SyncSourceFiles
            FilesFoldersContentSynchronizer.SyncSourceFiles(sourceFilesData, replicaFilesData, tmpFolderPath, logFilePathName);

            //collect the content of tmp-<guid>
            var contentInTemporaryFolder = FolderProcessor.MapFolderContent(tmpFolderPath);
            List<string> tmpFiles = contentInTemporaryFolder.files;
            List<string> tmpSubfolders = contentInTemporaryFolder.subfolders;
            List<FileDataModel> tmpFilesData = Helpers.GetFilesData(tmpFiles, tmpFolderPath);
            List<SubfolderDataModel> tmpSubfoldersData = Helpers.GetSubfoldersData(tmpSubfolders, tmpFolderPath);

            //rename replica to replicaBackup
            string replicaBackupFolderPath = replicaFolderPath + "Backup";
            try
            {
                Directory.Move(replicaFolderPath, replicaBackupFolderPath);
            }
            catch (Exception RpcFolderOpenException)
            {
                Logger.WriteLogToConsoleLogfile(logFilePathName, Environment.NewLine + Constants.ReplicaFileFolderOpenError);
                Logger.WriteLogToConsoleLogfile(logFilePathName, RpcFolderOpenException.ToString());
                Directory.Delete(tmpFolderPath, true);
                Environment.Exit(1);
            }
            //rename tmp to replica
            try
            {
                Directory.Move(tmpFolderPath, replicaFolderPath);
                syncedReplicaFolderPath = replicaFolderPath;
            }
            catch (Exception TmpFolderOpenException)
            {
                Logger.WriteLogToConsoleLogfile(logFilePathName, Environment.NewLine + tmpFolderPath + Constants.TmpFileFolderOpenError);
                Logger.WriteLogToConsoleLogfile(logFilePathName, TmpFolderOpenException.ToString());
                try
                {
                    Directory.Move(replicaBackupFolderPath, replicaFolderPath);
                }
                catch (Exception BackupFolderOpenException)
                {
                    Logger.WriteLogToConsoleLogfile(logFilePathName, Constants.ReplicaBackupAvailable + replicaBackupFolderPath);
                    Logger.WriteLogToConsoleLogfile(logFilePathName, BackupFolderOpenException.ToString());
                    Environment.Exit(1);
                }
            }
            //introduce new variables for synced replica folder content to avoid confusion
            //collect the content of syncedReplicaFolder
            //if all went well by now, then it will be the same content as the tmp-guid file (diff only in absolute paths)
            var contentInSyncedReplicaFolder = FolderProcessor.MapFolderContent(syncedReplicaFolderPath);
            List<string> syncedReplicaFiles = contentInSyncedReplicaFolder.files;
            List<string> syncedReplicaSubfolders = contentInSyncedReplicaFolder.subfolders;
            List<FileDataModel> syncedReplicaFilesData = Helpers.GetFilesData(syncedReplicaFiles, syncedReplicaFolderPath);
            List<SubfolderDataModel> syncedReplicaSubfoldersData = Helpers.GetSubfoldersData(syncedReplicaSubfolders, syncedReplicaFolderPath);

            //tidy the new replica folders' modified date and return the new subfoldersData
            syncedReplicaSubfoldersData = FilesFoldersContentSynchronizer.SetNewReplicaSubfoldersLastWriteTime(sourceSubfoldersData, syncedReplicaSubfoldersData);

            //verify that synced replica matches source
            try
            {
                FilesFoldersVerifier.VerifyFoldersMatch(sourceFilesData, syncedReplicaFilesData, sourceSubfoldersData, syncedReplicaSubfoldersData, syncIntervalMinute, syncedReplicaFolderPath);
                //delete replicaBackup
                Directory.Delete(replicaBackupFolderPath, true);
                Logger.WriteLogToConsoleLogfile(logFilePathName, Constants.SyncFinishedSuccess);
                Console.WriteLine(Constants.LogFileIsAt+logFilePathName);
                Console.WriteLine(Constants.WaitingPressEsc);

            }
            catch (Exception e)
            {
                Logger.WriteLogToConsoleLogfile(logFilePathName, e.Message);
                Logger.WriteLogToConsoleLogfile(logFilePathName, Constants.ReplicaBackupAvailable + replicaBackupFolderPath);
            }
        }
    }
}
