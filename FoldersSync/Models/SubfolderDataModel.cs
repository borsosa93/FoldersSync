using FoldersSync.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoldersSync.Models
{
    public class SubfolderDataModel
    {
        public string SubfolderPath { get; }
        public string SubfolderRelativePath { get; }
        public DateTime SubfolderLastWriteTime { get; set; }

        public SubfolderDataModel(string subfolderPath, string rootFolder)
        {
            SubfolderPath = subfolderPath;
            SubfolderRelativePath = subfolderPath.Replace(rootFolder, string.Empty); 
            SubfolderLastWriteTime=Directory.GetLastWriteTimeUtc(subfolderPath);
        }
    }
}
