using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoldersSync
{
    public class Options
    {
        [Option('s', "sourceFolderPath", Required = true, HelpText = "Enter the source folder path")]
        public string SourcePath { get; set; }
        [Option('r', "replicaFolderPath", Required = true, HelpText = "Enter the replica folder path")]
        public string ReplicaPath { get; set; }
        [Option('t', "syncTimePeriodMin", Required = true, HelpText = "Enter the synchronization period in minute")]
        public double SyncTimePeriodMin { get; set; }
        [Option('l', "logFolderPath", Required = true, HelpText = "Enter the folder path where the log file will be created")]
        public string LogPath { get; set; }
    }
}
