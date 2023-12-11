using LocalFileManager;

public class AppSettings
{
    public List<string> folders { get; private set; }

    public string MainFolder => GetFolderSafe(0);
    public string ToCopyFolder => GetFolderSafe(1);
    public string ToMoveFolder => GetFolderSafe(2);
    public List<string> fileExtensions { get; private set; }
    public int refreshTime { get; private set; }
    public string rootPath { get; private set; }

    public AppSettings(IConfiguration configuration)
    {
        LoadSettings(configuration);
    }
    private void LoadSettings(IConfiguration configuraton)
    {
        try
        {
            IConfigurationSection settings = configuraton.GetSection("Settings");
            IConfigurationSection testEnvInitializer = settings.GetSection("TestEnvInitializer");
            folders = testEnvInitializer.GetSection("FoldersPath").Get<List<string>>() ?? new List<string>();
            fileExtensions = testEnvInitializer.GetSection("FileExtensions").Get<List<string>>() ?? new List<string>();
            refreshTime = settings.GetValue<int>("RefreshTime", 10 * 1000);
            rootPath = settings.GetValue<string>("RootPath", "");

        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error loading settings", ex);
        }
    }
    private string GetFolderSafe(int index)
    {
        return index < folders.Count ? folders[index] : string.Empty;
    }
}