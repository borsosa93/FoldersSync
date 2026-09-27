using System;
using System.Collections.Generic;
using System.Text;

namespace FoldersSync.Models
{
    public class UserInputModel
    {
        public string SourceFolderPath { get; set; }
        public string ReplicaFolderPath { get; set; }
        public double SyncIntervalMinute { get; set; }
        public string LogFileFolderPath { get; set; }
    }
}
