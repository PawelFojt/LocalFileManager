using LocalFileManager.Manager;
using Newtonsoft.Json;

namespace LocalFileManager.Settings
{
    public class TasksSettings
    {
        public List<CopyManager> copyManagers { get; set; } = new();
        public List<MoveManager> moveManagers { get; set; } = new();
        public List<DeleteManager> deleteManagers { get; set; } = new();

        private readonly AppSettings _settings;

        public TasksSettings(AppSettings settings) 
        {
            this._settings = settings;
        }
        public void GetSettings()
        {
            var json = File.ReadAllText($"{_settings.baseDirectory}Settings\\taskssettings.json");
            var tasks = JsonConvert.DeserializeObject<TasksSettings>(json)!;


            foreach (var task in tasks.copyManagers)
            {
                copyManagers.Add(new CopyManager
                    (task.fileExtensions,
                    $"{_settings.baseDirectory}{task.sourceFolderPath}",
                    $"{_settings.baseDirectory}{task.destFolderPath}"));
            }

            foreach (var task in tasks.moveManagers)
            {
                moveManagers.Add(new MoveManager
                    (task.fileExtensions,
                    $"{_settings.baseDirectory}{task.sourceFolderPath}",
                    $"{_settings.baseDirectory}{task.destFolderPath}"));
            }

            foreach (var task in tasks.deleteManagers)
            {
                deleteManagers.Add(new DeleteManager
                    (task.fileExtensions,
                    $"{_settings.baseDirectory}{task.sourceFolderPath}"));
            }
        }
    }
}
