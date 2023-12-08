using Serilog;

namespace LocalFileManager
{
    public class Worker : BackgroundService
    {
        private readonly Settings _settings;
        private readonly TestEnvInitializer _initializer;
        private readonly FileManager _fileManager;


        public Worker(Settings settings)
        {
            Log.Information("Application started");
            _settings = settings;
            _initializer = new TestEnvInitializer(_settings.Folders, _settings.FileExtensions);
            _copyManager = new CopyManager(_settings.FileExtensions, _settings.ToCopyFolder, _settings.MainFolder);
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
