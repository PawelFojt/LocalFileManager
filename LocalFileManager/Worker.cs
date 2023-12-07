using Serilog;
using System.Runtime;

namespace LocalFileManager
{
    public class Worker : BackgroundService
    {
        private readonly static string[] _folders = new string[3];
        private readonly Settings _settings;
        private readonly FileManager _fileManager;


        public Worker()
        {
            Log.Information("Application started");
            IConfiguration config = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
           .Build();
            _settings = new Settings(config);
            _fileManager = new FileManager(_settings.Folders, _settings.FileExtension);
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
                Log.CloseAndFlush();
            }
        }
    }
}
