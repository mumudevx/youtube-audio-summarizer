namespace YouTubeDownloader.Services;

public interface IFileService
{
    public Task<string> SaveSummaryToFile(string summary, string fileName);
}

public class FileService : IFileService
{
    public async Task<string> SaveSummaryToFile(string summary, string fileName)
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), $"{fileName}.md");

        await File.WriteAllTextAsync(filePath, summary);

        return filePath;
    }
}

