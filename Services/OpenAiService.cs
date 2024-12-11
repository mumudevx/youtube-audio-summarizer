using System.Diagnostics.CodeAnalysis;
using OpenAI.Assistants;
using OpenAI.Audio;

namespace YouTubeDownloader.Services;

public interface IOpenAiService
{
    public Task<string> GetTranscriptionAsSrt(string audioFilePath);
    public string GetSummarizationOfContent(string content);
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

    [Experimental("OPENAI001")]
    public string GetSummarizationOfContent(string content)
    {
        var client = new OpenAIClient(openAiSettings.ApiKey);
        var assistantClient = client.GetAssistantClient();

        var threadOptions = new ThreadCreationOptions
        {
            InitialMessages = { content }
        };

        var threadRunResult = assistantClient.CreateThreadAndRun(openAiSettings.TranscriptionModel, threadOptions);

        do
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));
            threadRunResult = assistantClient.GetRun(threadRunResult.Value.ThreadId, threadRunResult.Value.Id);
        } while (!threadRunResult.Value.Status.IsTerminal);

        var messages = assistantClient
            .GetMessages(threadRunResult.Value.ThreadId,
                new MessageCollectionOptions { Order = MessageCollectionOrder.Ascending });

        var messageOfAssistant = messages
            .SelectMany(threadMessage => threadMessage.Content)
            .Aggregate("", (current, messageContent) => current + messageContent);

        return messageOfAssistant;
    }
}