using Serilog;
namespace LocalFileManager.Manager
{
    public class DeleteManager : FileManager
    {
        public DeleteManager(List<string> fileExtensions, string sourceFolderPath)
            : base(fileExtensions, sourceFolderPath) { }
        public void DeleteFiles()
        {
            ProcessFiles((source, dest) => File.Delete(source), "Deleted");
        }
    }
}
