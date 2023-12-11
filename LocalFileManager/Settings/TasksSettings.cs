using LocalFileManager.Manager;

namespace LocalFileManager.Settings
{
    internal class TasksSettings
    {
        public string sourceFolder { get; set; }
        public string destinationFolder { get; set; }
        public List<string> fileExtensions { get; set; }
    }
}
