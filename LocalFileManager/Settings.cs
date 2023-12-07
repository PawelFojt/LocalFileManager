internal class Settings
{
    public List<string> Folders { get; private set; }

    public string MainFolder => GetFolderSafe(0);
    public string ToCopyFolder => GetFolderSafe(1);
    public string ToMoveFolder => GetFolderSafe(2);
    public List<string> FileExtensions { get; private set; }
    public int RefreshTime { get; private set; }

    public Settings(IConfiguration configuration)
    {
        LoadSettings(configuration);
    }

    private void LoadSettings(IConfiguration configuraton)
    {
        try
        {
            IConfigurationSection settings = configuraton.GetSection("Settings");
            Folders = settings.GetSection("FoldersPath").Get<List<string>>() ?? new List<string>();
            FileExtensions = settings.GetSection("FileExtensions").Get<List<string>>() ?? new List<string>();
            RefreshTime = settings.GetValue<int>("RefreshTime", 10 * 1000);

        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error loading settings", ex);
        }
    }
    private string GetFolderSafe(int index)
    {
        return index < Folders.Count ? Folders[index] : string.Empty;
    }
}