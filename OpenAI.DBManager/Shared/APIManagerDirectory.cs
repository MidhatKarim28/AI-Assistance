
namespace OpenAI.DBManager
{
    internal sealed class APIManagerDirectory
    {
        private const string OPENAI_TURBO_MANAGER_CLASS = "OpenAITurboManager";
        private const string TOKEN_MANAGER_CLASS = "TokenManager";
        private const string ENCRYPTED_PASSWORD_MANAGER_CLASS = "EncryptedPasswordManager";
        public static string GetEncryptedPassword { get { return ENCRYPTED_PASSWORD_MANAGER_CLASS; } }
        public static string GetAIToken { get { return TOKEN_MANAGER_CLASS; } }
        public static string GetAIClinicalNote { get { return OPENAI_TURBO_MANAGER_CLASS; } }
    }
}
