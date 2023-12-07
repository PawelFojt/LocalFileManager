﻿using Serilog;

namespace LocalFileManager
{
    internal class FileManager
    {

        private readonly string test = "";
        private string fileExtension {  get; set; }
        public FileManager(string[] folders, string fileExtension) 
        {
            this.fileExtension = fileExtension;
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
            string[] files = Directory.GetFiles(sourceFolder, $"*{fileExtension}");

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
                using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
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

        private void InitializeFolders(string[] folders)
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
                    string filePath = Path.Combine(folderName, $"{Path.GetRandomFileName()}{fileExtension}");

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
