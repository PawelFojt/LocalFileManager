using Serilog;

namespace LocalFileManager
{
    public class Worker : BackgroundService
    {
        private readonly Settings _settings;
        private readonly FileManager _fileManager;


        public Worker(IConfiguration configuration)
        {
            Log.Information("Application started");
            _settings = new Settings(configuration);
            _fileManager = new FileManager(_settings.Folders, _settings.FileExtensions);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    _fileManager.CopyFiles(_settings.ToCopyFolder, _settings.MainFolder);
                    _fileManager.MoveFiles(_settings.ToMoveFolder, _settings.ToCopyFolder);
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
