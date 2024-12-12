#pragma warning disable OPENAI001

const string videoId = "GH_JLA-fkBY";

var settingsManagement = new SettingsManager();
var openAiSettings = settingsManagement.OpenAiSettings;

// Services
var youTubeService = new YouTubeService();
var openAiService = new OpenAiService(openAiSettings);
var fileService = new FileService();

// Get Audio File From YouTube
var audioFilePath = await youTubeService.DownloadAudioFromVideoId(videoId);

// Get Transcription Content From OpenAI Whisper
if (audioFilePath != null)
{
    var transcriptionContent = await openAiService.GetTranscriptionAsSrt(audioFilePath);

    //TODO: Write srt file to storage or hold in memory until summarize it? (for long videos, Currently in-memory)
    Console.WriteLine(transcriptionContent);

    // Get Summarization of Video via OpenAI Assistant
    var summarizationOfContent = openAiService.GetSummarizationOfContent(transcriptionContent);

    // Save Summarization to File
    var summaryFilePath = await fileService.SaveSummaryToFile(summarizationOfContent, "summary");

    Console.WriteLine($"Summary saved to {summaryFilePath}");
}