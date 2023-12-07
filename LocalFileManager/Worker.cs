using Serilog;

namespace LocalFileManager
{
    public class Worker : BackgroundService
    {
        private readonly static string[] _folders = 
        { 
            "folders\\Main",
            "folders\\ToCopy",
            "folders\\ToMove" 
        };
        private readonly string _destinationFolder = _folders[0];
        private readonly string _sourceFolder = _folders[1];
        private readonly string _moveFolder = _folders[2];
        private readonly FileManager _fileManager;


        public Worker()
        {
            Log.Information("Application started");
            _fileManager = new FileManager(_folders, ".csv");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {   
                    _fileManager.CopyFiles(_sourceFolder, _destinationFolder);
                    _fileManager.MoveFiles(_moveFolder, _sourceFolder);
                    await Task.Delay(5000, stoppingToken);
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
