using FoldersSync.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoldersSync
{
    public static class Helpers
    {
        public static List<FileDataModel> GetFilesData(List<string> files, string rootFolder)
        {
            List<FileDataModel> filesInFolderData = new List<FileDataModel>();
            foreach (var file in files)
            {
                FileDataModel fileModel = new FileDataModel(file, rootFolder);
                filesInFolderData.Add(fileModel);
            }
            return filesInFolderData;
        }

        public static List<SubfolderDataModel> GetSubfoldersData(List<string> subfolders, string rootFolder)
        {
            List<SubfolderDataModel> subfoldersData= new List<SubfolderDataModel>();
            foreach (var subfolder in subfolders) 
            {
                SubfolderDataModel subfolderModel= new SubfolderDataModel(subfolder, rootFolder);
                subfoldersData.Add(subfolderModel);
            }
            return subfoldersData;
        }

        public static string GetOpenedFile(List<string> files)
        {
            foreach (var file in files) 
            {
                FileInfo fi= new FileInfo(file);
                try
                {
                    using (FileStream stream = fi.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        stream.Close();
                    }
                }
                catch (IOException)
                {
                    return file;
                }
            }
            return null;
        }
    }
}
