var settingsManagement = new SettingsManager();
var openAiSettings = settingsManagement.OpenAiSettings;

const string videoId = "GH_JLA-fkBY";

// Get Audio File From YouTube
var youTubeService = new YouTubeService();
var audioFilePath = await youTubeService.DownloadAudioFromVideoId(videoId);

// Get Transcription Content From OpenAI Whisper
if (audioFilePath != null)
{
    var openAiService = new OpenAiService(openAiSettings);
    var transcriptionContent = await openAiService.GetTranscriptionAsSrt(audioFilePath);
    
    //TODO: Write srt file to storage or hold in memory until summarize it?
    Console.WriteLine(transcriptionContent);
}
