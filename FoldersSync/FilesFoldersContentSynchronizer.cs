using FoldersSync.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoldersSync
{
    public static class FilesFoldersContentSynchronizer
    {
        public static async Task SyncReplicaFiles(List<FileDataModel> sourceFilesData, List<FileDataModel> replicaFilesData, string logFilePathName)
        {
            foreach (var replicaFile in replicaFilesData)
            {
                bool replicaFileFoundInSource=false;
                foreach (var sourceFile in sourceFilesData)
                {
                    if (replicaFile.FileRelativePathName.Equals(sourceFile.FileRelativePathName))
                    {
                        replicaFileFoundInSource = true;
                        break;
                    }
                    else continue;
                }
                if (!replicaFileFoundInSource)
                {
                    await Logger.WriteLogToConsoleLogfile(logFilePathName, replicaFile.FilePathName + Constants.DeletedFromRpc_FileNotInSrc);
                }
            }
        }

        public static async Task SyncReplicaSubfolders(List<SubfolderDataModel> sourceSubfoldersData, List<SubfolderDataModel> replicaSubfoldersData, string logFilePathName)
        {
            foreach (var replicaSubfolder in replicaSubfoldersData)
            {
                bool replicaSubfolderFoundInSource = false;
                foreach (var sourceSubfolder in sourceSubfoldersData)
                {
                    if (replicaSubfolder.SubfolderRelativePath.Equals(sourceSubfolder.SubfolderRelativePath))
                    {
                        replicaSubfolderFoundInSource = true;
                        break;
                    }
                    else continue;
                }
                if (!replicaSubfolderFoundInSource)
                {
                    await Logger.WriteLogToConsoleLogfile(logFilePathName, replicaSubfolder.SubfolderRelativePath + Constants.DeletedFromRpc_SubfolderNotInSrc);
                }
            }
        }

        public static async Task SyncSourceSubfolders(List<SubfolderDataModel> sourceSubfoldersData, List<SubfolderDataModel> replicaSubfoldersData, string tmpFolderPath, string logFilePathName)
        {
            foreach (var sourceSubfolder in sourceSubfoldersData)
            {
                Directory.CreateDirectory(tmpFolderPath + sourceSubfolder.SubfolderRelativePath);
                bool sourceSubfolderFoundInReplica = false;
                foreach (var replicaSubfolder in replicaSubfoldersData)
                {
                    if (sourceSubfolder.SubfolderRelativePath.Equals(replicaSubfolder.SubfolderRelativePath))
                    {
                        sourceSubfolderFoundInReplica = true;
                        break;
                    }
                    else continue;
                }
                if (!sourceSubfolderFoundInReplica) 
                {
                    await Logger.WriteLogToConsoleLogfile(logFilePathName, sourceSubfolder.SubfolderRelativePath + Constants.CopiedToRpc_SubfolderNotInRpc);
                }
            }
        }

        public static async Task SyncSourceFiles(List<FileDataModel> sourceFilesData, List<FileDataModel> replicaFilesData, string tmpFolderPath, string logFilePathName)
        {
            foreach (var sourceFile in sourceFilesData)
            {
                File.Copy(sourceFile.FilePathName, tmpFolderPath+sourceFile.FileRelativePathName);
                bool sourceFileFoundInReplica = false;
                foreach (var replicaFile in replicaFilesData)
                {
                    if (sourceFile.FileRelativePathName.Equals(replicaFile.FileRelativePathName))
                    {
                        sourceFileFoundInReplica = true;
                        if (sourceFile.FileLastModifiedTimestamp.Equals(replicaFile.FileLastModifiedTimestamp)
                            && sourceFile.FileSize.Equals(replicaFile.FileSize))
                        {
                            break;
                        }
                        else 
                        {
                            await Logger.WriteLogToConsoleLogfile(logFilePathName, replicaFile.FileRelativePathName + Constants.VersionModified);
                            break;
                        }
                    }
                }
                if (!sourceFileFoundInReplica)
                {
                    await Logger.WriteLogToConsoleLogfile(logFilePathName, sourceFile.FileRelativePathName + Constants.CopiedToRpc_FileNotInRpc);
                }
            }
        }

        public static List<SubfolderDataModel> SetNewReplicaSubfoldersLastWriteTime(List<SubfolderDataModel> sourceSubfoldersData, List<SubfolderDataModel> replicaSubfoldersData)
        {
             List<SubfolderDataModel> updatedLastWriteReplicaSubfoldersData= new List<SubfolderDataModel>();
             foreach (var sourceSubfolder in sourceSubfoldersData)
                {
                    foreach (var replicaSubfolder in replicaSubfoldersData)
                    {
                        if (sourceSubfolder.SubfolderRelativePath.Equals(replicaSubfolder.SubfolderRelativePath))
                        {
                            replicaSubfolder.SubfolderLastWriteTime = sourceSubfolder.SubfolderLastWriteTime;
                            updatedLastWriteReplicaSubfoldersData.Add(replicaSubfolder);
                            break;
                        }
                    }
                }
             return updatedLastWriteReplicaSubfoldersData;
        }
    }
}
