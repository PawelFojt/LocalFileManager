using LocalFileManager.Manager;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;

namespace LocalFileManager
{
    public class Worker : BackgroundService
    {
        private readonly AppSettings _settings;
        private readonly TestEnvInitializer _initializer;
        private readonly FileManager _fileManager;


        public Worker(AppSettings settings)
        {
            Log.Information("Application started");
            _settings = settings;
            new TestEnvInitializer(_settings.Folders, _settings.FileExtensions);
            
            using StreamReader reader = new("taskssettings.json");
            var json = reader.ReadToEnd();
            dynamic tasks = JsonConvert.DeserializeObject(json);
            
            foreach ( var task in tasks.copyFiles )
            {
                string a = task.sourcePath;
                Log.Information($"{a}");
            }

        }

        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    new CopyManager(_settings.FileExtensions, _settings.ToCopyFolder, _settings.MainFolder);
                    await Task.Delay(_settings.RefreshTime, stoppingToken);
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                Log.Error(ex.Message);
            }
            catch (TaskCanceledException ex)
            {
                Log.Error(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error($"An error occurred: {ex}");
            }
            finally
            {
                Log.Information("Application stopped");
            }
        }
    }
}
