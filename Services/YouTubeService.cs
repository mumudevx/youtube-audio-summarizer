namespace YouTubeDownloader.Services;

public interface IYouTubeService
{
    public Task<string?> DownloadAudioFromVideoId(string videoId);
}

public class YouTubeService : IYouTubeService
{
    public async Task<string?> DownloadAudioFromVideoId(string videoId)
    {
        try
        {
            var youTubeClient = new YoutubeClient();

            var streamManifest = await youTubeClient.Videos.Streams.GetManifestAsync(videoId);

            var audioStreamInfo = streamManifest
                .GetAudioOnlyStreams()
                .OrderByDescending(stream => stream.Bitrate)
                .FirstOrDefault();

            if (audioStreamInfo != null)
            {
                var outputFilePath = $"{videoId}_low_quality.mp3";

                await youTubeClient.Videos.Streams.DownloadAsync(audioStreamInfo, outputFilePath);

                Console.WriteLine($"Video {videoId} audio downloaded successfully.");

                return outputFilePath;
            }

            Console.WriteLine("There is no audio stream info.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }

        return null;
    }
}