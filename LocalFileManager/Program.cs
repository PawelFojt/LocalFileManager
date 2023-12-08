using LocalFileManager;
using Serilog;
using System.Runtime;

public class Program
{
    public readonly Settings? _settings;
    private static async Task Main(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .Build();
        Settings settings = new (configuration);

        Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File($"Logs\\log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

        try
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddSingleton(settings);
                    services.AddHostedService<Worker>();
                })
                .UseSerilog()
                .Build();

            await host.RunAsync();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "An unexpected error occurred");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}