using FoldersSync.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;

namespace FoldersSync
{
    internal static class ArgumentValidator
    {
        private static string _sourceFolderPath;
        private static string _replicaFolderPath;
        private static double _syncIntervalMinute;
        private static string _logFileFolderPath;

        public static bool ValidateSrcFolderArg(string srcFolder)
        {
            if (!Directory.Exists(srcFolder))
            {
                Console.WriteLine(srcFolder+Constants.ArgFolderNotValid);
                Environment.Exit(-1);
            }
            return true;
        }
        public static bool ValidateRpcFolderArg(string srcFolder, string rpcFolder)
        {
            if (!Directory.Exists(srcFolder))
            {
                Console.WriteLine(srcFolder + Constants.ArgFolderNotValid);
                Environment.Exit(-1);
            }
            if (srcFolder.Equals(rpcFolder)) 
            {
                Console.WriteLine(Constants.RpcIsSrc);
                Environment.Exit(-1);
            }
            validateFolderNotSubfolder(srcFolder, rpcFolder, "source folder", "replica folder");
            return true;
        }
        public static bool ValidateTimeArg(double time)
        {
            if (time <= 0)
            {
                Console.WriteLine(Constants.ArgTimeNotValid);
                Environment.Exit(-1);
            }
            return true;
        }
        public static bool ValidateLogFolderArg(string srcFolder, string rpcFolder, string logFolder)
        {
            if (!Directory.Exists(logFolder))
            {
                Console.WriteLine(logFolder + Constants.ArgFolderNotValid);
                Environment.Exit(-1);
            }
            if (logFolder.Equals(srcFolder) || logFolder.Equals(rpcFolder))
            {
                Console.WriteLine(Constants.LogIsSrcRpc);
                Environment.Exit(-1);
            }
            validateFolderNotSubfolder(srcFolder, logFolder, "source folder", "log folder");
            validateFolderNotSubfolder(rpcFolder, logFolder, "replica folder", "log folder");

            return true;
        }

        private static void validateFolderNotSubfolder(string canParentFolder, string canChildFolder, string canParentFolderRole, string canChildFolderRole)
        {
            DirectoryInfo possibleParent = new DirectoryInfo(canParentFolder);
            DirectoryInfo possibleChild = new DirectoryInfo(canChildFolder);
            bool isParent = false;
            while (possibleChild.Parent != null)
            {
                if (possibleChild.Parent.FullName == possibleParent.FullName)
                {
                    isParent = true;
                    Console.WriteLine(canChildFolderRole + Constants.IsSubOf + canParentFolderRole);
                    Environment.Exit(-1);
                }
                else possibleChild = possibleChild.Parent;
            }
        }
    }
}
