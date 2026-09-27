using FoldersSync.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoldersSync
{
    public class FilesFoldersVerifier
    {
        public static void VerifyFoldersMatch(List<FileDataModel> sourceFilesData, List<FileDataModel> syncedReplicaFilesData, List<SubfolderDataModel> sourceSubfoldersData, List<SubfolderDataModel> syncedReplicaSubfoldersData, double syncIntervalMin, string syncedReplicaFolderPath)
        {
            verifyFilesMatch(sourceFilesData, syncedReplicaFilesData, syncedReplicaFolderPath);
            verifySubfoldersMatch(sourceSubfoldersData, syncedReplicaSubfoldersData, syncedReplicaFolderPath);
        }

        private static void verifyFilesMatch(List<FileDataModel> sourceFilesData, List<FileDataModel> syncedReplicaFilesData, string syncedReplicaFolderPath)
        {
            foreach (var sourceFile in sourceFilesData)
            {
                bool fileFound = false;
                foreach (var syncedReplicaFile in syncedReplicaFilesData)
                {
                    if (syncedReplicaFile.FileRelativePathName.Equals(sourceFile.FileRelativePathName))
                    {
                        fileFound = true;
                        if (!(syncedReplicaFile.FileLastModifiedTimestamp.Equals(sourceFile.FileLastModifiedTimestamp)))
                        {
                            throw new Exception(sourceFile.FileRelativePathName + Constants.LastModifiedDoesntMatch);
                        }
                        if (!(syncedReplicaFile.FileSize.Equals(sourceFile.FileSize)))
                        {
                            throw new Exception(sourceFile.FileRelativePathName + Constants.SizeDoesntMatch);
                        }
                        break;
                    }
                }
                if (!fileFound)
                {
                    throw new Exception(syncedReplicaFolderPath + sourceFile.FileRelativePathName + Constants.FileNotFoundInRpc);
                }
            }
        }

        private static void verifySubfoldersMatch(List<SubfolderDataModel> sourceSubfoldersData, List<SubfolderDataModel> syncedReplicaSubfoldersData, string syncedReplicaFolderPath)
        {
            foreach (var sourceSubfolder in sourceSubfoldersData)
            {
                bool subfolderFound = false;
                foreach (var syncedReplicaSubfolder in syncedReplicaSubfoldersData)
                {
                    if (syncedReplicaSubfolder.SubfolderRelativePath.Equals(sourceSubfolder.SubfolderRelativePath))
                    {
                        subfolderFound = true;
                        if (!(syncedReplicaSubfolder.SubfolderLastWriteTime.Equals(sourceSubfolder.SubfolderLastWriteTime)))
                        {
                            throw new Exception(sourceSubfolder.SubfolderRelativePath + Constants.LastModifiedDoesntMatch);
                        }
                        break;
                    }
                }
                if (!subfolderFound)
                {
                    throw new Exception(syncedReplicaFolderPath + sourceSubfolder.SubfolderRelativePath + Constants.SubfolderNotFoundInRpc);
                }
            }
        }
    }
}
