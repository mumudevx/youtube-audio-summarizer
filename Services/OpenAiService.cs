using OpenAI.Audio;

namespace YouTubeDownloader.Services;

public interface IOpenAiService
{
    public Task<string> GetTranscriptionAsSrt(string audioFilePath);
    public Task<string> GetSummarizationOfContent(string content);
}

public class OpenAiService(OpenAiSettings openAiSettings) : IOpenAiService
{
    public async Task<string> GetTranscriptionAsSrt(string audioFilePath)
    {
        var client = new OpenAIClient(openAiSettings.ApiKey);
        var audioClient = client.GetAudioClient(openAiSettings.TranscriptionModel);

        var transcriptionResult = await audioClient.TranscribeAudioAsync(
            audioFilePath: audioFilePath,
            new AudioTranscriptionOptions
            {
                ResponseFormat = AudioTranscriptionFormat.Srt
            }
        );

        return transcriptionResult.Value.Text;
    }

    public Task<string> GetSummarizationOfContent(string content)
    {
        throw new NotImplementedException();
    }
}