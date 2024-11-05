
namespace Shared.DBManager
{
    internal sealed class APIManagerDirectory
    {
        private const string TOKEN_MANAGER_CLASS = "TokenManager";

        public static string GetOneTimeCode { get { return TOKEN_MANAGER_CLASS; } }
    }
}
