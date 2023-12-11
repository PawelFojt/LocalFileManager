using Serilog;
namespace LocalFileManager.Manager
{
    internal class CopyManager : FileManager
    {
        public CopyManager(List<string> fileExtensions, string sourceFolderPath, string destFolderPath)
        {
            this.sourceFolderPath = sourceFolderPath;
            this.destFolderPath = destFolderPath;
            this.fileExtensions = fileExtensions;
            CopyFiles();
        }
        public void CopyFiles()
        {
            var files = GetFiles();

            foreach (string sourceFileName in files)
            {
                string fileName = Path.GetFileName(sourceFileName);
                string destFileName = Path.Combine(destFolderPath, fileName);

                if (File.Exists(destFileName)) continue;

                if (IsFileLocked(new FileInfo(sourceFileName)))
                {
                    Log.Warning($"File {sourceFileName} is in use.");
                    continue;
                }
                
                File.Copy(sourceFileName, destFileName);
                LogFileAction("Copied", fileName, sourceFileName);
                
            }

        }

    }
}
