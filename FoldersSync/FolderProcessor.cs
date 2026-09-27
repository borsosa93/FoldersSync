using FoldersSync.Models;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace FoldersSync
{
    internal class FolderProcessor
    {
        public static (List<string> files, List<string> subfolders) MapFolderContent(string targetFolder)
        {
            List<string> filesInFolder = new List<string>();
            List<string> subfoldersInFolder = new List<string>();
            getContentInFolder(targetFolder, filesInFolder, subfoldersInFolder);
            return (files:filesInFolder, subfolders:subfoldersInFolder);
        }

        private static void getContentInFolder(string targetFolder, List<string> files, List<string> subfolders)
        {
            // Process the list of files found in the folder
            string[] filesInFolder = Directory.GetFiles(targetFolder);
            foreach (string filePathName in filesInFolder)
            {
                files.Add(filePathName);
            }
                
            // Recurse into subfolders of this folder
            string[] subfolderEntries = Directory.GetDirectories(targetFolder);
            foreach (string subfolder in subfolderEntries)
            {
                subfolders.Add(subfolder);
                getContentInFolder(subfolder, files, subfolders);
            }    
                
        }

        public static string CreateTemporaryFolder(string logFileFolderPath, string logFileGuid)
        {
            string tmpFolderPath = logFileFolderPath + "\\" + "tmp-"+logFileGuid;
            Directory.CreateDirectory(tmpFolderPath);
            return tmpFolderPath;
        }
    }
}
