using Serilog;
namespace LocalFileManager.Manager
{
    public class MoveManager : FileManager
    {
        public MoveManager(List<string> fileExtensions, string sourceFolderPath, string destFolderPath)
            : base(fileExtensions, sourceFolderPath, destFolderPath) { }
        public void MoveFiles()
        {
            ProcessFiles((source, dest) => File.Move(source, dest), "Moved");
        }
    }
}
