using LocalFileManager;
using Serilog;

public class Program
{
    private static async Task Main(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .Build();

        Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File($"Logs\\log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

        try
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddSingleton(configuration);
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