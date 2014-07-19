using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FileSystemServices
{
    public class Explorer : FileSystemServices.IExplorer 
    {
        /// <summary>
        /// Gets the list of files 
        /// </summary>
        /// <param name="directory">The Directory to get the files from</param>
        /// <returns>Returns the List of File info for this directory.
        /// Return null if an exception is raised</returns>
        public IList<FileInfo> GetChildFiles(string directory)
        {
            try
            {
                return (from x in Directory.GetFiles(directory)
                        select new FileInfo(x)).ToList();
            }
            catch { }

            return null;
        }


        /// <summary>
        /// Gets the list of directories 
        /// </summary>
        /// <param name="directory">The Directory to get the files from</param>
        /// <returns>Returns the List of directories info for this directory.
        /// Return null if an exception is raised</returns>
        public IList<DirectoryInfo> GetChildDirectories(string directory)
        {
            try
            {
                return (from x in Directory.GetDirectories(directory)
                        select new DirectoryInfo(x)).ToList();
            }
            catch { }

            return null;
        }

        /// <summary>
        /// Gets the root directories of the system
        /// </summary>
        /// <returns>Return the list of root directories</returns>
        public IList<DirectoryInfo> GetRootDirectories()
        {
            return (from x in Directory.GetLogicalDrives()
                    select new DirectoryInfo(x)).ToList();
        }
    }
}
