using Serilog;

namespace LocalFileManager.Manager
{
    internal abstract class FileManager
    {
        protected string sourceFolderPath { get; set; }
        protected string destFolderPath { get; set; }
        protected List<string> fileExtensions { get; set; }

        public FileManager() { }
        public FileManager(List<string> fileExtensions, string sourceFolderPath, string destFolderPath)
        {
            this.sourceFolderPath = sourceFolderPath;
            this.destFolderPath = destFolderPath;
            this.fileExtensions = fileExtensions;
        }

        protected IEnumerable<string> GetFiles()
        {
            if (!Directory.Exists(sourceFolderPath))
            {
                return Enumerable.Empty<string>();
            }

            return Directory
                .GetFiles(sourceFolderPath)
                .Where(file => fileExtensions.Any(file.ToLower().EndsWith));
        }

        protected void LogFileAction(string action, string fileName, string filePath)
        {
            string logMessage = $"{action} file: {fileName} from {sourceFolderPath} {(destFolderPath == null ? "" : "to " + destFolderPath)}," +
                $" Size: {new FileInfo(filePath).Length} bytes";
            Log.Information(logMessage);
        }

        protected bool IsFileLocked(FileInfo file)
        {
            try
            {
                using (FileStream stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                return true;
            }
            return false;
        }
    }
}
