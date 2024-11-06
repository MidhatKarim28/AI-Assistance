using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text;
using NAudio.Wave;
using OpenAI_API;
using OpenAI_API.Audio;
using System.Speech.Recognition;

namespace AIAssistanceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RealTimeTranscriptionController : ControllerBase
    {
        private readonly ILogger<RealTimeTranscriptionController> _logger;
        private readonly OpenAIAPI _openAI;

        public RealTimeTranscriptionController(ILogger<RealTimeTranscriptionController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _openAI = new OpenAIAPI(configuration["OpenAI:ApiKey"]);
        }

        [HttpGet("/ws")]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await Echo(webSocket);
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }

        private async Task Echo(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            var receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);

            try
            {
                using (var recognizer = new SpeechRecognitionEngine())
                {
                    recognizer.SetInputToDefaultAudioDevice();
                    recognizer.LoadGrammar(new DictationGrammar());

                    recognizer.SpeechRecognized += async (s, e) =>
                    {
                        var transcription = e.Result.Text;
                        await webSocket.SendAsync(
                            new ArraySegment<byte>(Encoding.UTF8.GetBytes(transcription)),
                            WebSocketMessageType.Text,
                            true,
                            CancellationToken.None);
                    };

                    recognizer.RecognizeAsync(RecognizeMode.Multiple);

                    while (!receiveResult.CloseStatus.HasValue)
                    {
                        await webSocket.ReceiveAsync(
                            new ArraySegment<byte>(buffer), CancellationToken.None);
                    }

                    recognizer.RecognizeAsyncStop();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in WebSocket communication");
            }
            finally
            {
                await webSocket.CloseAsync(
                    receiveResult.CloseStatus.GetValueOrDefault(),
                    receiveResult.CloseStatusDescription,
                    CancellationToken.None);
            }
        }

        [HttpPost("generate-notes")]
        public async Task<IActionResult> GenerateNotes([FromBody] GenerateNotesRequest request)
        {
            try
            {
                var chatRequest = new OpenAI_API.Chat.ChatRequest()
                {
                    Model = "gpt-3.5-turbo",
                    Messages = new[]
                    {
                        new OpenAI_API.Chat.ChatMessage(OpenAI_API.Chat.ChatMessageRole.System, "You are a medical professional. Summarize the following conversation into clinical notes."),
                        new OpenAI_API.Chat.ChatMessage(OpenAI_API.Chat.ChatMessageRole.User, request.Transcription)
                    }
                };

                var result = await _openAI.Chat.CreateChatCompletionAsync(chatRequest);
                return Ok(result.Choices[0].Message.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating clinical notes");
                return StatusCode(500, "An error occurred while generating clinical notes");
            }
        }
    }

    public class GenerateNotesRequest
    {
        public string Transcription { get; set; }
        public string PatientContext { get; set; }
    }
}
