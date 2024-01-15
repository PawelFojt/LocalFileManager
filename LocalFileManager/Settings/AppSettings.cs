public class AppSettings
{
    public int refreshTime { get; private set; }
    public string baseDirectory { get; private set; } = null!;


    public AppSettings(IConfiguration configuration)
    {
        LoadSettings(configuration);
    }
    private void LoadSettings(IConfiguration configuraton)
    {
        try
        {
            IConfigurationSection settings = configuraton.GetSection("Settings");
            refreshTime = settings.GetValue<int>("RefreshTime", 10 * 1000);
            baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error loading settings", ex);
        }
    }
}