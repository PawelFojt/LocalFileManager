using LocalFileManager.Settings;
using Serilog;

namespace LocalFileManager
{
    public class Worker : BackgroundService
    {
        private readonly AppSettings _settings;
        private readonly TasksSettings _tasksSettings;

        public Worker(AppSettings settings)
        {
            Log.Information("Application started");
            _settings = settings;
            _tasksSettings = new TasksSettings(settings);
            _tasksSettings.GetSettings();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    foreach (var copyManager in _tasksSettings.copyManagers)
                    {
                        copyManager.CopyFiles();
                    }
                   
                    foreach (var moveManager in  _tasksSettings.moveManagers)
                    {
                        moveManager.MoveFiles();
                    }

                    foreach (var deleteManager in _tasksSettings.deleteManagers)
                    {
                        deleteManager.DeleteFiles();
                    }

                    await Task.Delay(_settings.refreshTime, stoppingToken);
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
