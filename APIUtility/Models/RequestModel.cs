using System.Collections.Generic;

namespace APIUtility.Models
{
    public class RequestModel 
    {
        public string RequestMethod { get; set; }
        public object RequestData { get; set; }
        public string RequestDateTime { get; set; }
        public string TokenHeader { get; set; }
        public List<FileModel> files { get; set; }
        public bool IsPostRequest { get; set; }

        public RequestModel(string requestMethod, object requestData, string requestDateTime, string tokenHeader, 
            List<FileModel> uploadedFiles = null, bool isPostRequest = true)
        {
            RequestMethod = requestMethod;
            RequestData = requestData;
            RequestDateTime = requestDateTime;
            TokenHeader = tokenHeader;
            files = uploadedFiles;
            IsPostRequest = isPostRequest;
        }
    }
}