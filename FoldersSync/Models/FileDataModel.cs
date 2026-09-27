using System;
using System.Collections.Generic;
using System.Text;

namespace FoldersSync.Models
{
    public class FileDataModel
    {
        public string FilePathName { get; }
        public string FileRelativePathName { get; }
        public long FileSize { get; }
        public DateTime FileLastModifiedTimestamp { get; }

        public FileDataModel(string fileName, string rootFolder)
        {
            FilePathName = fileName;
            FileRelativePathName= fileName.Replace(rootFolder, string.Empty);
            FileLastModifiedTimestamp =File.GetLastWriteTimeUtc(fileName);
            FileSize = new FileInfo(fileName).Length;
        }
    }
}
