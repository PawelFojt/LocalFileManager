using Serilog;

namespace LocalFileManager
{
    internal abstract class FileManager
    {
        protected string sourceFolder { get; set; }
        protected string destinationFolder { get; set; }
        protected List<string> fileExtensions { get; set; }

        public FileManager() { }
        public FileManager(List<string> fileExtensions, string sourceFolder, string destinationFolder) 
        {
            this.sourceFolder = sourceFolder;
            this.destinationFolder = destinationFolder;
            this.fileExtensions = fileExtensions;
        }

        protected IEnumerable<string> GetFiles(string sourceFolder)
        {
            return Directory
                .GetFiles(sourceFolder)
                .Where(file => fileExtensions.Any(file.ToLower().EndsWith));
        }

        protected void LogFileAction(string action, string fileName, string sourceFolder, string destinationFolder, string filePath)
        {
            string logMessage = $"{action} file: {fileName} from {sourceFolder} to {destinationFolder}, Size: {new FileInfo(filePath).Length} bytes";
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
