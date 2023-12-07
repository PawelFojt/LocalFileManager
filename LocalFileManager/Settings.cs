internal class Settings
{
    public List<string> FoldersList { get; private set; }

    public string[] Folders {  get; private set; }
    public string MainFolder => FoldersList.Count > 0 ? FoldersList[0] : string.Empty;
    public string ToCopyFolder => FoldersList.Count > 1 ? FoldersList[1] : string.Empty;
    public string ToMoveFolder => FoldersList.Count > 2 ? FoldersList[2] : string.Empty;
    public string FileExtension { get; private set; }
    public int RefreshTime { get; private set; }

    public Settings(IConfiguration config)
    {
        LoadSettings(config);
    }

    private void LoadSettings(IConfiguration config)
    {
        try
        {
            IConfigurationSection settings = config.GetSection("Settings");
            FoldersList = settings.GetSection("FoldersPath").Get<List<string>>() ?? new List<string>();
            Folders = FoldersList.ToArray();
            FileExtension = settings.GetValue<string>("FileExtension", string.Empty);
            RefreshTime = settings.GetValue<int>("RefreshTime", 60); // Defaulting to 60 if not set

        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error loading settings", ex);
        }
    }
}