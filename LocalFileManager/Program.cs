using LocalFileManager;
using Serilog;

public class Program
{
    private static async Task Main(string[] args)
    {
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        IConfigurationSection section = config.GetSection("Settings");



        string setting1 = section["Setting1"];
        string setting2 = section["Setting2"];
        Log.Information(setting1 + " " + setting2);


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