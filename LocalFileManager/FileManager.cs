using Serilog;

namespace LocalFileManager
{
    internal class FileManager
    {
        
        private List<string> fileExtensions {  get; set; }
        public FileManager(List<string> folders, List<string> fileExtensions) 
        {
            this.fileExtensions = fileExtensions;
            InitializeFolders(folders);
        }
        
        private enum FileOperation
        {
            Move,
            Copy
        }
        public void CopyFiles(string sourceFolder, string destinationFolder)
        {
            ProcessFiles(sourceFolder, destinationFolder, FileOperation.Copy);
            
        }

        public void MoveFiles(string sourceFolder, string destinationFolder)
        {
            ProcessFiles(sourceFolder, destinationFolder, FileOperation.Move);
        }

        private void ProcessFiles(string sourceFolder, string destinationFolder, FileOperation fileOperation)
        {
            var files = Directory
                .GetFiles(sourceFolder)
                .Where(file => fileExtensions.Any(file.ToLower().EndsWith));

            foreach (var file in files)
            {
                string fileName = Path.GetFileName(file);
                string destinationPath = Path.Combine(destinationFolder, fileName);

                if (File.Exists(destinationPath)) continue;

                switch (fileOperation)
                {
                    case FileOperation.Move:
                        if (IsFileLocked(new FileInfo(file)))
                        {
                            Log.Warning($"{file} is in use");
                            continue;
                        }
                        LogFileAction("Moved", fileName, sourceFolder, destinationFolder, file);
                        File.Move(file, destinationPath);
                        break;
                    case FileOperation.Copy:
                        LogFileAction("Copied", fileName, sourceFolder, destinationFolder, file);
                        File.Copy(file, destinationPath);    
                        break;
                    default:
                        throw new ArgumentException("Invalid file operation method.");
                }
            }
        }

        private void LogFileAction(string action, string fileName, string sourceFolder, string destinationFolder, string filePath)
        {
            string logMessage = $"{action} file: {fileName} from {sourceFolder} to {destinationFolder}, Size: {new FileInfo(filePath).Length} bytes";
            Log.Information(logMessage);
        }

        private bool IsFileLocked(FileInfo file)
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

        private void InitializeFolders(List<string> folders)
        {
            try
            {
                foreach (string folder in folders)
                {
                    CreateFolderIfNotExist(folder);
                }
                InitializeFiles(3, folders[1]);
                InitializeFiles(2, folders[2]);
                Log.Information("Initialization completed");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

        }

        private void CreateFolderIfNotExist(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                Log.Information($"{folderPath} folder created");
            }
            else
            {
                Log.Warning($"{folderPath} folder already exist");
            }
        }

        private void InitializeFiles(int numberOfFiles, string folderName)
        {
            if (numberOfFiles <= 0)
            {
                throw new ArgumentException("Number of files must be greater than zero.");
            }

            if (Directory.Exists(folderName))
            {
                for (int i = 1; i <= numberOfFiles; i++)
                {
                    var random = new Random();
                    int randomExtension = random.Next(fileExtensions.Count);
                    string filePath = Path.Combine(folderName, $"{Path.GetRandomFileName()}{fileExtensions[randomExtension]}");

                    if (!File.Exists(filePath))
                    {
                        File.WriteAllText(filePath, $"Sample content for file {i}");
                        Log.Information($"Created sample file: {filePath}");
                    }
                }
            }
            else
            {
                Log.Warning($"{folderName} folder doesn't exist");
            }
        }
    }
}
