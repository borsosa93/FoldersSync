using System;
using System.Collections.Generic;
using System.Text;

namespace FoldersSync
{
    public static class Constants
    {
        public static string Header { get; }
        public static string EscapeCommand { get; }
        public static string TerminateBeforeSyncStarted { get; }
        public static string ApplicationStarted {  get;  }
        public static string SyncStartsIn { get; }
        public static string WaitingPressEsc { get; }
        public static string SyncStarted { get; }
        public static string ShuttingDownApplication { get; }
        public static string TimerStopped { get; }
        public static string ShutdownSignalReceived { get; }
        public static string ApplicationTerminated { get; }
        public static string LogFileNamePrefix { get; }
        public static string ReplicaFileFolderOpenError { get; }
        public static string ReplicaBackupAvailable { get; }
        public static string DeletedFromRpc_FileNotInSrc { get; }
        public static string DeletedFromRpc_SubfolderNotInSrc { get; }
        public static string CopiedToRpc_FileNotInRpc { get; }
        public static string CopiedToRpc_SubfolderNotInRpc { get; }
        public static string VersionModified { get; }
        public static string LastModifiedDoesntMatch { get; }
        public static string SizeDoesntMatch { get; }
        public static string FileNotFoundInRpc { get; }
        public static string SubfolderNotFoundInRpc { get; }
        public static string TmpFileFolderOpenError { get; }
        public static string SyncFinishedSuccess { get; }
        public static string LogFileIsAt { get; }
        public static string EnterSrc {  get; }
        public static string EnterRpc { get; }
        public static string EnterIntervalMin { get; }
        public static string EnterLog { get; }
        public static string RpcIsSrc { get; }
        public static string LogIsSrcRpc { get; }
        public static string IsSubOf { get; }
        public static string ArgFolderNotValid { get; }
        public static string ArgTimeNotValid { get; }

        static Constants()
        {
            Header = "===================================================================\n                     FOLDER SYNCHRONIZER\n-------------------------------------------------------------------\n   Periodically synchronizes a source folder with a replica folder\n===================================================================\n\nARGUMENTS\n-------------------------------------------------------------------\n  1. -s source folder path\n  2. -r  replica folder path\n  3. -t synchronization period in minutes\n  4. -l log folder path\n\n  Press Esc anytime to stop the application\n\n  The replica folder will be periodically updated to match\n  the content of the source folder\n\nNOTE\n-------------------------------------------------------------------\n  * Make sure no files in the replica folder and subfolders\n    are open and no subfolders of the replica folder are open\n    before starting the synchronization\n\n  * Files in the source folder and its subfolders, and\n    subfolders of the source folder can remain open\n\n  * Logs are written to a text file created in the specified\n    log folder and are also displayed in the console\n\n  * If a synchronization fails, the files in the source folder\n    remain available in their original location. Replica folder\n    content may be left intact or found in a backup folder\n    in the parent folder of the replica folder\n\n-------------------------------------------------------------------"; EscapeCommand = "escape";
            TerminateBeforeSyncStarted = "Esc pressed! Terminating the application before the first synchonization had started";
            SyncStarted = "Synchronization started";
            WaitingPressEsc = "Waiting for next synchronization. Press Esc to quit";
            ShuttingDownApplication = "Esc pressed! Shutting down the application...";
            TimerStopped = "Timer stopped successfully";
            ShutdownSignalReceived="Shutdown signal received. Cleaning up resources...";
            ApplicationTerminated = "Application terminated by user";
            LogFileNamePrefix = "log_sync_";
            ReplicaFileFolderOpenError ="Synchronization failed: One or more files or subfolders of the replica folder are opened by another process. Close the files or subfolders and restart the application";
            ReplicaBackupAvailable= "A backup of the original replica folder is available at ";
            DeletedFromRpc_FileNotInSrc = " was deleted from the replica folder: The file doesn't exist in the source folder";
            DeletedFromRpc_SubfolderNotInSrc= " was deleted from the replica folder: The subfolder doesn't exist in the source folder";
            CopiedToRpc_SubfolderNotInRpc=" was copied to the replica folder: The subfolder doesn't exist in the replica folder";
            CopiedToRpc_FileNotInRpc = " was copied to the replica folder: The file doesn't exist in the replica folder";
            VersionModified=" was modified to match the version in the source folder: File size or last save time doesn't match";
            LastModifiedDoesntMatch=" last modified timestamp doesn't match in the source and in the replica folder";
            SizeDoesntMatch=" file size doesn't match in the source and in the replica folder";
            FileNotFoundInRpc = ": a copy of the file was not found in the replica folder";
            SubfolderNotFoundInRpc = ": a copy of the subfolder was not found in the replica folder";
            TmpFileFolderOpenError = "Synchronization failed: One or more files or subfolders of the tmp folder are opened by another process. Close the files or subfolders and restart the application";
            SyncFinishedSuccess = "Synchronization finished sucessfully";
            LogFileIsAt = "Log file is saved at ";
            RpcIsSrc = "Replica folder can't be the same as source folder";
            LogIsSrcRpc = "Log file folder can't be the same as source folder or replica folder";
            IsSubOf = " folder can't be the subfolder of the ";
            ArgFolderNotValid = " folder doesn't exist";
            ArgTimeNotValid = " is not a valid time value";
        }
    }
}
