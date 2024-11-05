using APIUtility.Constants;
using APIUtility.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OpenAI.Models;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAi_Assistant.Models;
using OpenAi_Assistant.Services;
using System.Net;
using Utility;
using static OpenAI_API.Audio.TextToSpeechRequest;

namespace OpenAI.DBManager
{
    public class OpenAITurboManager
    {
        //Added by MK for Clinical Notes
        public object GetAIClinicalNote(RequestModel request)
        {
            SuccessResponse successResponseModel = new SuccessResponse();
            try
            {
                ChatModel chatModel = JsonConvert.DeserializeObject<ChatModel>(Convert.ToString(request.RequestData));
                var openai = new OpenAIAPI(ApplicationSettings.Instance.AppLocalSetting.OpenAIKey);

                // Call getNote method to get the audio stream
                string audioToTextTranscription = getNote(openai, chatModel.AudioFilePath).Result;

                string updatedMessageWithInstructions = $@"
                Please create a clinical note from the following transcription:

                {audioToTextTranscription}.

                Format:
                Name: [Patient's Name]
                Date of Birth: [DOB]
                MRN: [Medical Record Number]
                Date of Visit: [Date]
                Provider: [Provider's Name]
                Chief Complaint: [Brief description of the patient's main concern]
                History of Present Illness: [Details about the current issue, including duration, symptoms, and any relevant history]
                Review of Systems: [Summary of other systems reviewed that may relate to the patient's condition]
                Diagnosis: [Primary and any secondary diagnoses]
                Treatment Plan: [Details on medications, therapies, referrals, or follow-up appointments]
                Patient Education: [Information provided to the patient about their condition and treatment]
                Follow-Up: [When the patient should return for further evaluation]";
                ChatRequest chatRequest = new ChatRequest();

                //if (chatModel.QueryHistory == null)
                //    chatModel.QueryHistory = new List<string>();

                //chatModel.QueryHistory.Insert(chatModel.QueryHistory.Count, chatModel.Query);

                //ChatMessage newMsg = new ChatMessage(ChatMessageRole.User, updatedMessageWithInstructions);

                // Create a new list to hold ChatMessage instances
                List<ChatMessage> newMessages = new List<ChatMessage>();

                // Create a new ChatMessage instance
                ChatMessage newMsg = new ChatMessage(ChatMessageRole.User, updatedMessageWithInstructions);

                // Add the new message to the list
                newMessages.Add(newMsg);

                // Assign the list to chatRequest.Messages
                chatRequest.Messages = newMessages;

                chatRequest.Model = OpenAI_API.Models.Model.ChatGPTTurbo;
                chatRequest.MaxTokens = 1000;
                chatRequest.Temperature = 0.5; //0.7

                var completions = openai.Chat.CreateChatCompletionAsync(chatRequest).Result;
                string outputResult = string.Empty;

                foreach (var completion in completions.Choices)
                {
                    outputResult += completion.Message.Content;
                }
                chatModel.ResultContent = outputResult;

                ChatResponseModel chatResponseModel = new ChatResponseModel()
                {
                    Query = chatModel.AudioFilePath,
                    ResultContent = chatModel.ResultContent // Pass the base64 encoded audio to your Web App
                };

                successResponseModel = new SuccessResponse(chatResponseModel, true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
                return new ErrorResponse(ex.Message, HttpStatusCode.BadRequest);
            }
            return successResponseModel;
        }
        public async Task<string> getNote(OpenAIAPI openai, string audioFilePath)
        {
            try
            {
                // Get the transcription asynchronously
                var transcriptionResult = openai.Transcriptions.GetTextAsync(audioFilePath).Result;

                return transcriptionResult;
            }
            catch (Exception ex)
            {
                // Log the error and rethrow or handle it
                Logger.WriteErrorLog(ex);
                throw new InvalidOperationException("Error generating the note.", ex);
            }
        }

    }
}
