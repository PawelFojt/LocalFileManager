using Serilog;

namespace LocalFileManager.Manager
{
    public abstract class FileManager
    {
        public string sourceFolderPath { get; set; } = "";
        public string destFolderPath { get; set; } = "";
        public List<string> fileExtensions { get; set; } = new();

        public FileManager() { }

        public FileManager(List<string> fileExtensions, string sourceFolderPath)
            : base()
        {
            this.sourceFolderPath = sourceFolderPath;
            this.fileExtensions = fileExtensions;
        }
        public FileManager(List<string> fileExtensions, string sourceFolderPath, string destFolderPath)
            : base()
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
            string logMessage = $"{action} file: {fileName} from {sourceFolderPath} {(destFolderPath == "" ? "" : "to " + destFolderPath)}," +
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

        public delegate void FileOperation(string sourceFileName, string destFileName);

        public void ProcessFiles(FileOperation fileOperation, string fileOperationMethodName)
        {
            var sourceFilesName = GetFiles();

            foreach (string sourceFileName in sourceFilesName)
            {
                string fileName = Path.GetFileName(sourceFileName);
                string destFileName = Path.Combine(destFolderPath, fileName);

                if (File.Exists(destFileName)) continue;

                if (IsFileLocked(new FileInfo(sourceFileName)))
                {
                    Log.Warning($"File {sourceFileName} is in use.");
                    continue;
                }

                try
                {
                    LogFileAction(fileOperationMethodName, fileName, sourceFileName);
                    fileOperation(sourceFileName, destFileName);
                }
                catch (IOException ex)
                {
                    Log.Error($"Error in {fileOperationMethodName.ToLower()} {fileName}: {ex.Message}");
                }
            }
        }
    }
}
