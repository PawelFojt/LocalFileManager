using LocalFileManager.Manager;
using Newtonsoft.Json;
using Serilog;

namespace LocalFileManager.Settings
{
    public class TasksSettings
    {
        public List<CopyManager> copyManagers { get; set; } = new();
        public List<MoveManager> moveManagers { get; set; } = new();
        public List<DeleteManager> deleteManagers { get; set; } = new();

        public void GetSettings()
        {
            var json = File.ReadAllText("Settings\\taskssettings.json");
            var tasks = JsonConvert.DeserializeObject<TasksSettings>(json);

            foreach (var task in tasks.copyManagers)
            {
                copyManagers.Add(new CopyManager(task.fileExtensions, task.sourceFolderPath, task.destFolderPath));
            }

            foreach (var task in tasks.moveManagers)
            {
                moveManagers.Add(new MoveManager(task.fileExtensions, task.sourceFolderPath, task.destFolderPath));
            }

            foreach (var task in tasks.deleteManagers)
            {
                deleteManagers.Add(new DeleteManager(task.fileExtensions, task.sourceFolderPath));
            }
        }
    }
}
