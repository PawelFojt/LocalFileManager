using LocalFileManager;
using Serilog;

public class Program
{
    private static async Task Main(string[] args)
    {
        IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    .UseSerilog()
    .Build();

        Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File($"Logs\\log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

        await host.RunAsync();
    }
}