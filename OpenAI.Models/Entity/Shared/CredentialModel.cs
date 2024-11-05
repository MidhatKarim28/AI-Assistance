using Utility;

namespace OpenAI.Models
{
    public class CredentialModel : CommonDictionaryAttribute
    {
        public string UserID { get; set; }
        public string Password { get; set; }   
    }
    
}
