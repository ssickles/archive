using System;
namespace FileSystemServices
{
    public interface IExplorer
    {
        System.Collections.Generic.IList<System.IO.DirectoryInfo> GetChildDirectories(string directory);
        System.Collections.Generic.IList<System.IO.FileInfo> GetChildFiles(string directory);
        System.Collections.Generic.IList<System.IO.DirectoryInfo> GetRootDirectories();
    }
}
