using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalFileManager
{
    internal class CopyManager : FileManager
    {
       
     


        public void CopyFiles(string destinationPath)
        {
            var files = GetFiles(sourceFolder);

            foreach (var filePath in files)
            {
                if (File.Exists(filePath) && !IsFileLocked(filePath))
                {
                    string fileName = Path.GetFileName(filePath);
                    File.Move(filePath, destinationPath);
                    LogFileAction("Moved", fileName, sourceFolder, destinationFolder, filePath);
                }
            }
            
        }

    }
}
