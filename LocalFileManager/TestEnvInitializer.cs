using Serilog;
using System.Runtime;

namespace LocalFileManager
{
    internal class TestEnvInitializer
    {
        private readonly List<string> _fileExtensions;
        public TestEnvInitializer(List<string> foldersPath, List<string> fileExtensions) 
        {
            _fileExtensions = fileExtensions;
            InitializeFolders(foldersPath);
        }

        private void InitializeFolders(List<string> foldersPath)
        {
            try
            {
                foreach (string folderPath in foldersPath)
                {
                    CreateFolderIfNotExist(folderPath);
                }
                InitializeFiles(3, foldersPath[1]);
                InitializeFiles(2, foldersPath[2]);
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
                    int randomExtension = random.Next(_fileExtensions.Count);
                    string filePath = Path.Combine(folderName, $"{Path.GetRandomFileName()}{_fileExtensions[randomExtension]}");

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
