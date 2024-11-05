using Utility;

namespace OpenAI.Models
{
    public class ChatModel : CredentialModel
    {
        public string AudioFilePath { get; set; }
        public string Query { get; set; }
        public List<string> QueryHistory { get; set; }
        public string ResultContent { get; set; }
    }

    public class ChatResponseModel
    {
        public string Query { get; set; }
        public List<string> QueryHistory { get; set; }
        public string ResultContent { get; set; }
        public string AssistantID { get; set; }
        public string AssistantName { get; set; }
        public string ThreadID { get; set; }
    }
    
}
