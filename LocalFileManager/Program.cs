using LocalFileManager;
using Serilog;

public class Program
{
    public readonly AppSettings? _settings;

    private static async Task Main(string[] args)
    {
        IConfiguration globalConfiguration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .Build();

        AppSettings globalSettings = new (globalConfiguration);      

        Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File($"Logs\\log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

        try
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddSingleton(globalSettings);
                    services.AddHostedService<Worker>();
                })
                .UseSerilog()
                .UseWindowsService()
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