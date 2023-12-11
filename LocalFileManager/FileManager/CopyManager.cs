using Serilog;
namespace LocalFileManager.Manager
{
    public class CopyManager : FileManager
    {
        public CopyManager(List<string> fileExtensions, string sourceFolderPath, string destFolderPath)
            : base(fileExtensions, sourceFolderPath, destFolderPath) { }
        public void CopyFiles()
        {
            ProcessFiles((source, dest) => File.Copy(source, dest), "Copied");
        }

    }
}
