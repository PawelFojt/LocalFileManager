namespace LocalFileManager
{
    internal class CopyManager : FileManager
    {
        public CopyManager(List<string> fileExtensions, string sourceFolder, string destinationFolder)
        {
            this.sourceFolder = sourceFolder;
            this.destinationFolder = destinationFolder;
            this.fileExtensions = fileExtensions;
            CopyFiles();
        }
        public void CopyFiles()
        {
            var files = GetFiles(sourceFolder);

            foreach (var filePath in files)
            {
                if (File.Exists(filePath) && !IsFileLocked(new FileInfo(filePath)))
                {
                    string fileName = Path.GetFileName(filePath);
                    File.Move(filePath, destinationFolder);
                    LogFileAction("Moved", fileName, sourceFolder, destinationFolder, filePath);
                }
            }
            
        }

    }
}
