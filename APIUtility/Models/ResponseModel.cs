using Newtonsoft.Json;
using System.Net;

namespace APIUtility.Models
{    
    public class ResponseModel
    {
        public string ResponseMessage;
        public HttpStatusCode ResponseCode;
        public string ResponseType;
        public object ResponseResult;

        [JsonConstructor]
        public ResponseModel(string message, HttpStatusCode code, string type, object result) {

            ResponseMessage = message;
            ResponseCode = code;
            ResponseType = type;
            ResponseResult = result;
        }
        
        public ResponseModel(string message, HttpStatusCode code){

            ResponseMessage = message;
            ResponseCode = code;
        }
    }
}