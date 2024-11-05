using System.Net;

namespace APIUtility.Models
{
    public class ErrorResponse
    {
        public string ErrorMessage;
        public HttpStatusCode ErrorCode;

        public ErrorResponse(string message, HttpStatusCode code)
        {
            ErrorMessage = message;
            ErrorCode = code;
        }
    }
}