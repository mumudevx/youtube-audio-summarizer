namespace YouTubeDownloader.Configuration;

public class SettingsManager
{
    public SettingsManager()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appSettings.Local.json", optional: false, reloadOnChange: true)
            .Build();
        
        config.GetSection("OpenAiSettings").Bind(OpenAiSettings);
    }
    
    public OpenAiSettings OpenAiSettings { get; } = new();
}