using System.Net;

namespace APIUtility.Models
{
    public class SuccessResponse
    {
        public string ResponseMessage;
        public HttpStatusCode ResponseCode;
        public object Data;

        public SuccessResponse()
        {
            ResponseCode = HttpStatusCode.NoContent;
        }

        public SuccessResponse(object data, bool hasData, string responseMessage = "")
        {
            ResponseMessage = responseMessage;
            if (hasData)
            {
                ResponseCode = HttpStatusCode.OK;
                Data = data;
            }
            else
            {
                ResponseCode = HttpStatusCode.NoContent;
                if (string.IsNullOrEmpty(responseMessage))
                {
                    ResponseMessage = Utility.Constants.Default_NoRecordFoundMessage;
                }
            }
        }
    }
}
